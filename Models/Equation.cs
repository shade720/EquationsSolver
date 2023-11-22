namespace EquationsSolver.Models;

public class Equation
{
    private readonly List<double> _coefficients;
    public IReadOnlyCollection<double> Coefficients => _coefficients;

    public Equation(IEnumerable<double> coefficients)
    {
        var localCoefficients = coefficients.ToList();
        if (localCoefficients is null)
            throw new ArgumentNullException(nameof(coefficients));
        if (localCoefficients.ToList().Count < 2)
            throw new ArgumentException("The number of coefficients is incorrect!");

        _coefficients = localCoefficients;
    }

    public override string ToString()
    {
        var coefficientsCount = _coefficients.Count;
        return string.Join(" + ", _coefficients.Select((x,i) => $"{x}x^{coefficientsCount - i}"));
    }
}