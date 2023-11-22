using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;

namespace EquationsSolver.ConsoleUI;

public class ConsoleEquationReaderFactory : IEquationReaderFactory
{
    private readonly IEquationParser _equationParser;

    public ConsoleEquationReaderFactory(IEquationParser equationParser)
    {
        _equationParser = equationParser;
    }

    public IEquationsReader CreateEquationsReader(string? equationsSourcePath)
    {
        if (!string.IsNullOrEmpty(equationsSourcePath) && equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            return new FileEquationsReader(equationsSourcePath, _equationParser);

        return new ConsoleEquationsReader(_equationParser);
    }
}