/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace NatsunekoLaboratory.EnhancedTransformEditor
{
    /**
     * Expression Evaluator for Unity
     * 
     * This evaluator supports to the following expressions:
     * * arithmetic operators: (+, -, *, /) and power (^), modulo (%)
     * * parentheses
     * * math functions: abs, acos, approximately, asin, atan, atan2, ceil, clamp, degrees, saturate (= clamp01), cos, exp, floor, frac, lerp, log, log10, max, min, pow, radians, round, sign, sin, smoothstep, sqrt, tan
     * * constants: PI, EPSILON
     * * variables: **customized**
     * 
     * For Example:
     * * this + 10
     * *
     */
    internal class ExpressionEvaluator
    {
        public static bool TryEvaluate(string expression, List<(string, Union<float, object>)> variables, List<CustomFunction> functions, out float value)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                value = float.NaN;
                return false;
            }

            if (TryParse(expression, out var expr) && TryEvaluate(expr, variables, functions, out var result))
            {
                value = result;
                return true;
            }

            value = float.NaN;
            return false;
        }

        public static float Evaluate(string expression, List<(string, Union<float, object>)> variables, List<CustomFunction> functions)
        {
            if (TryEvaluate(expression, variables, functions, out var result))
                return result;
            return float.NaN;
        }

        private static bool TryParse(string expression, out Queue<Expression> expressions)
        {
            var buffer = new Queue<Expression>();
            var stack = new Stack<Expression>();

            void EvaluateRank(OperationKind kind)
            {
                while (stack.Count > 0)
                {
                    var expr = stack.Peek();
                    if (expr is OperatorExpression o && EvaluateOperatorRank(kind) >= EvaluateOperatorRank(o.Operator))
                        buffer.Enqueue(stack.Pop());
                    else
                        break;
                }
            }

            try
            {
                using (var sr = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(expression))))
                {
                    while (sr.Peek() != -1)
                    {
                        var c = (char)sr.Read();

                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\r':
                            case '\n':
                                break;

                            case '+':
                                EvaluateRank(OperationKind.Add);
                                stack.Push(new OperatorExpression(OperationKind.Add));
                                break;

                            case '-':
                                EvaluateRank(OperationKind.Subtract);
                                stack.Push(new OperatorExpression(OperationKind.Subtract));
                                break;

                            case '*':
                                EvaluateRank(OperationKind.Multiply);
                                stack.Push(new OperatorExpression(OperationKind.Multiply));
                                break;

                            case '/':
                                EvaluateRank(OperationKind.Divide);
                                stack.Push(new OperatorExpression(OperationKind.Divide));
                                break;

                            case '%':
                                EvaluateRank(OperationKind.Modulo);
                                stack.Push(new OperatorExpression(OperationKind.Modulo));
                                break;

                            case '^':
                                stack.Push(new OperatorExpression(OperationKind.Power));
                                break;

                            case '(':
                                stack.Push(new ParenthesesExpression(false));
                                break;

                            case ')':
                            {
                                var k = stack.ToArray().Select((w, i) => (Value: w, Index: i)).First(w => w.Value is ParenthesesExpression e && !e.IsEnd).Index;
                                var s = new Stack<Expression>();
                                for (var j = 0; j <= k; j++)
                                {
                                    var expr = stack.Pop();
                                    if (expr is ParenthesesExpression)
                                        continue;

                                    s.Push(expr);
                                }

                                while (s.Count > 0)
                                    buffer.Enqueue(s.Pop());

                                break;
                            }

                            default:
                                if ('0' <= c && c <= '9')
                                {
                                    buffer.Enqueue(ParseAsNumeric(c, sr));
                                }
                                else if ('a' <= c && c <= 'z')
                                {
                                    buffer.Enqueue(ParseAsExpression(c, sr));
                                }
                                else if ('A' <= c && c <= 'Z')
                                {
                                    buffer.Enqueue(ParseAsConstant(c, sr));
                                }
                                else if (',' == c)
                                {
                                    // maybe not required below statements
                                    while (stack.Count > 0)
                                        buffer.Enqueue(stack.Pop());
                                    buffer.Enqueue(new SeparatorExpression());
                                }

                                break;
                        }
                    }

                    while (stack.Count > 0)
                        buffer.Enqueue(stack.Pop());
                }

                expressions = buffer;
                return true;
            }
            catch (Exception e)
            {
                // Debug.LogException(e);

                expressions = new Queue<Expression>();
                return false;
            }
        }

        private static Expression ParseAsNumeric(char c, StreamReader sr)
        {
            var sb = new StringBuilder();
            sb.Append(c);

            while (sr.Peek() != -1)
            {
                var peek = sr.Peek();
                if ('0' <= peek && peek <= '9' || peek == '.')
                    sb.Append((char)sr.Read());
                else
                    break;
            }

            return new NumericExpression(float.Parse(sb.ToString()));
        }

        private static Expression ParseAsExpression(char c, StreamReader sr)
        {
            var funcSb = new StringBuilder();
            funcSb.Append(c);

            // read function calls (or variables)
            while (sr.Peek() != -1)
            {
                var s = sr.Peek();
                if ('a' <= s && s <= 'z' || s == '_')
                    funcSb.Append((char)sr.Read());
                else
                    break;
            }

            if (sr.Peek() == '(')
                return new FunctionExpression(funcSb.ToString(), ParseInnerExpressions(sr));

            return new VariableExpression(funcSb.ToString());
        }

        private static List<Queue<Expression>> ParseInnerExpressions(StreamReader sr)
        {
            sr.Read(); // (

            var stack = new Stack<object>();
            var sb = new StringBuilder();

            while (sr.Peek() != -1)
            {
                var c = (char)sr.Peek();
                if (c == ')' && stack.Count == 0)
                {
                    sr.Read(); // )
                    break;
                }

                if (c == '(')
                    stack.Push(null);
                if (c == ')')
                    stack.Pop();

                sb.Append((char)sr.Read());
            }

            if (stack.Count != 0)
                throw new InvalidOperationException();

            TryParse(sb.ToString(), out var expressions);

            var arguments = new List<Queue<Expression>>();
            var argument = new Queue<Expression>();
            foreach (var expression in expressions.ToArray())
                if (expression is SeparatorExpression)
                {
                    arguments.Add(argument);
                    argument = new Queue<Expression>();
                }
                else
                {
                    argument.Enqueue(expression);
                }

            arguments.Add(argument);

            return arguments;
        }

        private static Expression ParseAsConstant(char c, StreamReader sr)
        {
            var sb = new StringBuilder();
            sb.Append(c);

            while (sr.Peek() != -1)
            {
                var peek = sr.Peek();
                if ('A' <= peek && peek <= 'Z' || peek == '_')
                    sb.Append((char)sr.Read());
                else
                    break;
            }

            switch (sb.ToString())
            {
                case "PI":
                    return new NumericExpression(Mathf.PI);

                case "EPSILON":
                    return new NumericExpression(Mathf.Epsilon);
            }

            throw new ArgumentOutOfRangeException($"`{sb}` is not valid constant");
        }

        private static bool TryEvaluate(Queue<Expression> expressions, List<(string, Union<float, object>)> variables, List<CustomFunction> functions, out float value)
        {
            var call = new Stack<NumericExpression>();

            while (expressions.Count > 0)
            {
                var expr = expressions.Dequeue();

                switch (expr)
                {
                    case NumericExpression n:
                        call.Push(n);
                        break;

                    case VariableExpression v:
                        try
                        {
                            call.Push(new NumericExpression(variables.First(w => w.Item1 == v.Variable).Item2.AsT1));
                        }
                        catch
                        {
                            value = float.NaN;
                            return false;
                        }

                        break;

                    case FunctionExpression f:
                        var arguments = new List<float>();
                        foreach (var argument in f.Arguments)
                            if (TryEvaluate(argument, variables, functions, out var v))
                            {
                                arguments.Add(v);
                            }
                            else
                            {
                                value = float.NaN;
                                return false;
                            }

                        try
                        {
                            call.Push(new NumericExpression(EvaluateFunctionCall(f, arguments.ToArray(), variables, functions)));
                        }
                        catch
                        {
                            value = float.NaN;
                            return false;
                        }

                        break;

                    case OperatorExpression o:
                        if (call.Count < 2)
                        {
                            value = float.NaN;
                            return false;
                        }

                        var a = call.Pop();
                        var b = call.Pop();

                        switch (o.Operator)
                        {
                            case OperationKind.Add:
                                call.Push(new NumericExpression(b.Numeric + a.Numeric));
                                break;

                            case OperationKind.Subtract:
                                call.Push(new NumericExpression(b.Numeric - a.Numeric));
                                break;

                            case OperationKind.Multiply:
                                call.Push(new NumericExpression(b.Numeric * a.Numeric));
                                break;

                            case OperationKind.Divide:
                                call.Push(new NumericExpression(b.Numeric / a.Numeric));
                                break;

                            case OperationKind.Power:
                                call.Push(new NumericExpression(Mathf.Pow(b.Numeric, a.Numeric)));
                                break;

                            case OperationKind.Modulo:
                                call.Push(new NumericExpression(b.Numeric % a.Numeric));
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                }
            }

            value = call.Pop().Numeric;
            return true;
        }

        private static float EvaluateFunctionCall(FunctionExpression expression, float[] arguments, List<(string, Union<float, object>)> variables, List<CustomFunction> functions)
        {
            switch (expression.Function)
            {
                case "abs":
                    return Mathf.Abs(arguments[0]);

                case "acos":
                    return Mathf.Acos(arguments[0]);

                case "approximately":
                    return Mathf.Approximately(arguments[0], arguments[1]) ? 1 : 0;

                case "asin":
                    return Mathf.Asin(arguments[0]);

                case "atan":
                    return Mathf.Atan(arguments[0]);

                case "atan2":
                    return Mathf.Atan2(arguments[0], arguments[1]);

                case "ceil":
                    return Mathf.Ceil(arguments[0]);

                case "clamp":
                    return Mathf.Clamp(arguments[0], arguments[1], arguments[2]);

                case "degrees":
                    return arguments[0] * Mathf.Deg2Rad;

                case "saturate":
                    return Mathf.Clamp01(arguments[0]);

                case "cos":
                    return Mathf.Cos(arguments[0]);

                case "exp":
                    return Mathf.Exp(arguments[0]);

                case "floor":
                    return Mathf.Floor(arguments[0]);

                case "frac":
                    return arguments[0] - Mathf.FloorToInt(arguments[0]);

                case "lerp":
                    return Mathf.Lerp(arguments[0], arguments[1], arguments[2]);

                case "log":
                    return arguments.Length == 1 ? Mathf.Log(arguments[0]) : Mathf.Log(arguments[0], arguments[1]);

                case "log10":
                    return Mathf.Log10(arguments[0]);

                case "max":
                    return Mathf.Max(arguments);

                case "min":
                    return Mathf.Min(arguments);

                case "pow":
                    return Mathf.Pow(arguments[0], arguments[1]);

                case "radians":
                    return arguments[0] * Mathf.Deg2Rad;

                case "round":
                    return Mathf.Round(arguments[0]);

                case "sign":
                    return Mathf.Sign(arguments[0]);

                case "sin":
                    return Mathf.Sin(arguments[0]);

                case "smoothstep":
                    return Mathf.SmoothStep(arguments[0], arguments[1], arguments[2]);

                case "sqrt":
                    return Mathf.Sqrt(arguments[0]);

                case "tan":
                    return Mathf.Tan(arguments[0]);

                default:
                    if (functions.Any(w => w.Name == expression.Function))
                        return functions.First(w => w.Name == expression.Function).Body.Invoke(arguments, variables);
                    throw new ArgumentOutOfRangeException(expression.Function);
            }
        }

        private static int EvaluateOperatorRank(OperationKind @operator)
        {
            if (@operator == OperationKind.Power)
                return 1;
            if (@operator == OperationKind.Multiply || @operator == OperationKind.Divide || @operator == OperationKind.Modulo)
                return 2;
            if (@operator == OperationKind.Add || @operator == OperationKind.Subtract)
                return 3;
            return 10;
        }

        public class CustomFunction
        {
            public string Name { get; }

            public Func<float[], List<(string, Union<float, object>)>, float> Body { get; }

            public CustomFunction(string name, Func<float[], List<(string, Union<float, object>)>, float> body)
            {
                Name = name;
                Body = body;
            }
        }

        private class Expression
        {
            public ExpressionKind Kind { get; }

            protected Expression(ExpressionKind kind)
            {
                Kind = kind;
            }
        }

        private class OperatorExpression : Expression
        {
            public OperationKind Operator { get; }

            public OperatorExpression(OperationKind @operator) : base(ExpressionKind.Operator)
            {
                Operator = @operator;
            }

            public override string ToString()
            {
                return Operator.ToString();
            }
        }

        private class ParenthesesExpression : Expression
        {
            public bool IsEnd { get; }

            public ParenthesesExpression(bool isEndParentheses) : base(ExpressionKind.Parentheses)
            {
                IsEnd = isEndParentheses;
            }

            public override string ToString()
            {
                return IsEnd ? ")" : "(";
            }
        }

        private class NumericExpression : Expression
        {
            public float Numeric { get; }

            public NumericExpression(float value) : base(ExpressionKind.Numeric)
            {
                Numeric = value;
            }

            public override string ToString()
            {
                return Numeric.ToString();
            }
        }

        private class VariableExpression : Expression
        {
            public string Variable { get; }

            public VariableExpression(string variable) : base(ExpressionKind.Variable)
            {
                Variable = variable;
            }

            public override string ToString()
            {
                return Variable;
            }
        }

        private class FunctionExpression : Expression
        {
            public string Function { get; }

            public List<Queue<Expression>> Arguments { get; }

            public FunctionExpression(string function, List<Queue<Expression>> arguments) : base(ExpressionKind.Function)
            {
                Function = function;
                Arguments = arguments;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append($"{Function}(");

                foreach (var argument in Arguments)
                {
                    foreach (var expression in argument)
                        sb.Append(expression);

                    sb.Append(", ");
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        private class SeparatorExpression : Expression
        {
            public SeparatorExpression() : base(ExpressionKind.Separator) { }
        }

        private enum ExpressionKind
        {
            Parentheses,

            Numeric,

            Operator,

            Function,

            Variable,

            Separator
        }

        private enum OperationKind
        {
            Add,

            Subtract,

            Multiply,

            Divide,

            Power,

            Modulo
        }
    }
}