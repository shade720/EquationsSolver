namespace EquationsSolver.Models;

public class EquationSolvingResult
{
    public bool IsSolvedSuccessful { get; set; }
    public Roots Roots { get; set; }
    public double? Root1 { get; set; }
    public double? Root2 { get; set; }
}