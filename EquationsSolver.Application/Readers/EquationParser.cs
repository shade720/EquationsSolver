using System.Globalization;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Exceptions;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.Application.Readers;

public class EquationParser : IEquationParser
{
    public Equation Parse(string coefficientLine)
    {
        if (string.IsNullOrEmpty(coefficientLine))
            throw new ArgumentNullException(coefficientLine, nameof(Parse));

        var parsedCoefficients = coefficientLine
            .Split(' ')
            .Aggregate(new List<double>(), (list, value) =>
            {
                if (double.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
                    list.Add(result);
                else
                    throw new EquationParseException($"Коэффициент '{value}' введен неверно.");
                return list;
            });

        return new Equation(parsedCoefficients);
    }
}