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

    public IEquationsReader CreateEquationsReader(string? equationsSourcePath)
    {
        // Проверяем, является ли источник файловым.
        if (!string.IsNullOrEmpty(equationsSourcePath) && equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            return new FileEquationsReader(equationsSourcePath, _equationParser);

        return new ConsoleEquationsReader(_equationParser);
    }
}