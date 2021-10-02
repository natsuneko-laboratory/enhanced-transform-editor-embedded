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

In the Enhanced Transform Editor, you can use many of the math functions contained in the `Mathf` class.
If there is a function you are missing, please feel free to contact me.


## Extra Functions

Extra Functions are not implemented as mathematical functions, but are functions that perform some processing. Many functions take `index` as an input value.

### `space_between(float, float)`

The `space_between` function places the objects at equal intervals.
This is used when you want to even out the space between objects.
The first argument is the space between each object, and the second argument is the `index` which indicates what object it is.


### `center(float, float)`

The `center` function moves the Transform so that it is centered at the position specified in the first argument for the currently selected element.
