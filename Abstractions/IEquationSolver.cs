using EquationsSolver.Models;

namespace EquationsSolver.Abstractions;

public interface IEquationSolver
{
    public EquationSolvingResult Solve(Equation equation);
}