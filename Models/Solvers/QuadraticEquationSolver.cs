using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Solvers;

public class QuadraticEquationSolver : IEquationSolver
{
    public EquationSolvingResult Solve(Equation equation)
    {
        if (equation.Coefficients.Count != 3)
            return new EquationSolvingResult { IsSolvedSuccessful = false };

        var a = equation.Coefficients.ElementAt(2);
        var b = equation.Coefficients.ElementAt(1);
        var c = equation.Coefficients.ElementAt(0);

        var discriminant = Math.Pow(b, 2) - 4 * a * c;

        if (discriminant < 0)
        {
            var root1 = (-b + Math.Sqrt(discriminant)) / 2 * a;
            var root2 = (-b - Math.Sqrt(discriminant)) / 2 * a;
            return new EquationSolvingResult
            {
                IsSolvedSuccessful = true,
                Roots = Roots.One,
                Root1 = root1,
                Root2 = root2
            };
        }

        const double epsilon = 0.00001;

        if (Math.Abs(discriminant) < epsilon)
        {
            var root = -b / 2 * a;
            return new EquationSolvingResult
            {
                IsSolvedSuccessful = true, 
                Roots = Roots.One, 
                Root1 = root,
                Root2 = root
            };
        }

        // discriminant < 0
        return new EquationSolvingResult { IsSolvedSuccessful = true, Roots = Roots.None };
    }
}