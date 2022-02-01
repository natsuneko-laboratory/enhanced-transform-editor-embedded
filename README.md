# Enhanced Transform Editor

Unity Editor Extension enhances the transform editor that supports math functions, variables, constants, and custom functions.

## Features

- Replace Transform Property Editor with the following features:
  - It provides the same functionality as the [ExpressionEvaluator](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/ExpressionEvaluator.html) provided in Unity 2021.2, but with a few additional features.
    - Arithmetic operators
    - Power and Module operators
    - Parentheses
    - Math Functions
      - abs, acos, approximately, asin, atan, atan2, ceil, clamp, degrees, saturate (= clamp01), cos, exp, floor, frac, lerp, log, log10, max, min, pow, radians, round, sign, sin, smoothstep, sqrt, tan
    - Constants
      - `PI` and `EPSILON`
    - Variables
      - `this` indicates the value of itself, `index` indicates the number of the object whose element is being calculated
    - Utility Functions
      - `space_between`, `center`, and others...
  - If you want to use evaluator, call `float ExpressionEvaluator.Evaluate(string,List<(string, Union<float, object>)>,List<CustomFunction>)`.

## Documentation

You can checkout the documentation in the [/docs](/docs) directory.

## License

This software is licensed under the License Zero Parity 7.0.0 and MIT license with exception License Zero Patron 1.0.0.
This is the same as the license for Husky 5.0, and can be summarized as follows:

- If you are using this software for open-source projects, you may use it under the terms of the MIT License.
- If you are using this software for commercial projects, you may use it under the terms of the License Zero Parity 7.0.0.
- If you are using this software for commercial projects, but you are supporting this project via GitHub Sponsors, Patreon, and others (monthly or yearly donations are supported), you may use it under the terms of the License Zero Patron 1.0.0.
- If you are contributing to this project, you SHOULD compliant with the MIT License.
