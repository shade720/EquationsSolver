using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.Application.Solvers;

public class QuadraticEquationSolver : IEquationSolver
{
    public EquationSolvingResult Solve(Equation equation)
    {
        if (equation.Coefficients.Count != 3 || equation.Coefficients.ElementAt(0) == 0)
            return new EquationSolvingResult { IsSolvedSuccessful = false, Roots = Array.Empty<double>(), OriginalEquation = equation };

        var a = equation.Coefficients.ElementAt(0);
        var b = equation.Coefficients.ElementAt(1);
        var c = equation.Coefficients.ElementAt(2);

        var discriminant = Math.Pow(b, 2) - 4 * a * c;

        if (discriminant > 0)
        {
            var root1 = (-b + Math.Sqrt(discriminant)) / 2 * a;
            var root2 = (-b - Math.Sqrt(discriminant)) / 2 * a;
            return new EquationSolvingResult
            {
                IsSolvedSuccessful = true,
                Roots = new[] { root1, root2 },
                OriginalEquation = equation
            };
        }

        const double epsilon = 0.00001;
        if (Math.Abs(discriminant) < epsilon)
        {
            var root = -b / 2 * a;
            return new EquationSolvingResult
            {
                IsSolvedSuccessful = true,
                Roots = new[] { root },
                OriginalEquation = equation
            };
        }

        // discriminant < 0
        return new EquationSolvingResult { IsSolvedSuccessful = true, Roots = Array.Empty<double>(), OriginalEquation = equation };
    }
}