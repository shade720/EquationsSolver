using EquationsSolver.Domain.Models;

namespace EquationsSolver.Domain.Abstractions;

public interface IEquationSolver
{
    public EquationSolvingResult Solve(Equation equation);
}