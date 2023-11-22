using EquationsSolver.Abstractions;
using EquationsSolver.Models.Readers;

namespace EquationsSolver.Factories;

public class EquationReaderFactory : IEquationReaderFactory
{
    private readonly IEquationParser _equationParser;

    public EquationReaderFactory(IEquationParser equationParser)
    {
        _equationParser = equationParser;
    }

    public IEquationsReader CreateEquationsReader(string? equationsPath)
    {
        if (!string.IsNullOrEmpty(equationsPath) && equationsPath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            return new FileEquationsReader(equationsPath, _equationParser);

        return new ConsoleEquationsReader(_equationParser);
    }
}