namespace EquationsSolver.Domain.Models;

public class Equation
{
    private readonly List<double> _coefficients;
    public IReadOnlyCollection<double> Coefficients => _coefficients;

    public Equation(IEnumerable<double> coefficients)
    {
        var localCoefficients = coefficients.ToList();
        if (localCoefficients is null)
            throw new ArgumentNullException(nameof(coefficients));
        if (localCoefficients.Count < 2)
            throw new ArgumentException($"Уравнение с данными коэффициентами невозможно. Количество коэффициентов: {localCoefficients.Count}");
        _coefficients = localCoefficients;
    }

    public override string ToString()
    {
        var degree = _coefficients.Count - 1;
        return $"{_coefficients[0]}x^{degree}{string.Join("", _coefficients.Skip(1).SkipLast(1).Select((c, i) => $" + ({c})x^{degree - i - 1}"))} + ({_coefficients[^1]})";
    }
}