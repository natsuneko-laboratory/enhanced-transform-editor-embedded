# Enhanced Transform Editor

Unity Editor Extension that enhance transform editor that support math functions, variables, constants and custom functions.

## Features

* Replace Transform Property Editor with the following features:
  * It provides the same functionality as the [ExpressionEvaluator](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/ExpressionEvaluator.html) provided in Unity 2021.2, but with a few additional features.
    * Arithmetic operators
    * Power and Module operators
    * Parentheses
    * Math Functions
      * abs, acos, approximately, asin, atan, atan2, ceil, clamp, degrees, saturate (= clamp01), cos, exp, floor, frac, lerp, log, log10, max, min, pow, radians, round, sign, sin, smoothstep, sqrt, tan
    * Constants
      * `PI` and `EPSILON`
    * Variables
      * `this` indicates the value of itself, `index` indicates the number of the object whose element is being calculated
    * Utility Functions
      * `space_between`, `center`, and others...
  * If you want to use evaluator, call `float ExpressionEvaluator.Evaluate(string,List<(string, Union<float, object>)>,List<CustomFunction>)`.

## Documentation

You can checkout the documentation in the [/docs](/docs) directory.

## License

MIT by [@6jz](https://twitter.com/6jz)
