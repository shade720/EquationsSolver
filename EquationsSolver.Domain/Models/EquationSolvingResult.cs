namespace EquationsSolver.Domain.Models;

public class EquationSolvingResult
{
    public Equation OriginalEquation { get; set; }
    public bool IsSolvedSuccessful { get; set; }
    public ICollection<double> Roots { get; set; }
}