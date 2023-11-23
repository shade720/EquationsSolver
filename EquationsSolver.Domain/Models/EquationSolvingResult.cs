namespace EquationsSolver.Domain.Models;

public class EquationSolvingResult
{
    public Equation OriginalEquation { get; init; }
    public bool IsSolvedSuccessful { get; init; }
    public ICollection<double> Roots { get; init; }
}