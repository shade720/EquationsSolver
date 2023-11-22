using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.Application.Readers;
public class FileEquationsReader : IEquationsReader
{
    private readonly IEquationParser _equationParser;
    private readonly string _filename;

    public FileEquationsReader(
        string filename,
        IEquationParser equationParser)
    {
        _filename = filename;
        _equationParser = equationParser;
    }

    public IEnumerable<Equation> Read()
    {
        using var sr = new StreamReader(_filename);
        while (!sr.EndOfStream)
        {
            var coefficientLine = sr.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
                continue;

            Equation parsedEquation;
            try
            {
                parsedEquation = _equationParser.Parse(coefficientLine);
            }
            catch (Exception)
            {
                continue;
            }

            yield return parsedEquation;
        }
    }
}