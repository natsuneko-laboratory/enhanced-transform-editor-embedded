# Expression Evaluator

Expression Evaluator is a class for decomposing and parsing strings, and executing and evaluating them as mathematical expressions.
This class is able to understand the following formulas and functions. It can also understand some additional variables and so on.

* Arithmetic operators (`+`, `-`, `*`, `/`)
* Power (`^`) and Module (`%`) operators
* Parentheses
* Math Functions
  * abs, acos, approximately, asin, atan, atan2, ceil, clamp, degrees, saturate (= clamp01), cos, exp, floor, frac, lerp, log, log10, max, min, pow, radians, round, sign, sin, smoothstep, sqrt, tan
* Constants
  * `PI` and `EPSILON`
* Variables
  * `this` indicates the value of itself, `index` indicates the number of the object whose element is being calculated
* Utility Functions
  * `space_between`, `center`, and others...


## Functions

### `space_between`

The `space_between` function places the objects at equal intervals.
This is used when you want to even out the space between objects.

* Signature : `space_between(float space, float index)`  
* Usage: `space_between(0.5, index)`
