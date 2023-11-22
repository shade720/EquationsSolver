using System.Globalization;
using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Readers;

public class EquationParser : IEquationParser
{
    public Equation? Parse(string coefficientLine)
    {
        var coefficients = coefficientLine
            .Split(' ')
            .ToList();
        var expectedCoefficientsCount = coefficients.Count;

        var parsedCoefficients = coefficients.Aggregate(new List<double>(), (list, value) =>
        {
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                list.Add(result);
            else
                Console.WriteLine($"Коэффициент '{value}' введен неверно.");
            return list;
        });

        return parsedCoefficients.Count == expectedCoefficientsCount 
            ? new Equation(parsedCoefficients) 
            : null;
    }
}