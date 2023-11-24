# EquationsSolver

This application is designed to solve quadratic equations by given coefficients. Coefficients are set from a file or console.

## Usage

Launch the application with console input
```
EquationsSolver
```
The path to the file where the equations are stored.
```
EquationsSolver -f ./equations.txt
```
Or
```
EquationsSolver --file ./equations.txt
```
Calculations will be performed in parallel in the specified number of threads. The parameter is not available for console input.
```
EquationsSolver -f ./equations.txt -p 4
```
Or
```
EquationsSolver --file ./equations.txt --parallel 4
```
Display this help screen.
```
EquationsSolver --help
```
Display version information.
```
EquationsSolver --version
```
## Technology stack:
* .NET 7
* Serilog.Sinks.Console
* Microsoft.DependencyInjection
* CommandLineParser
