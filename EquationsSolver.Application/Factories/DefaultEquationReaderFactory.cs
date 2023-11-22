using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;

namespace EquationsSolver.Application.Factories;

public class DefaultEquationReaderFactory : IEquationReaderFactory
{
    private readonly IEquationParser _equationParser;

    public DefaultEquationReaderFactory(IEquationParser equationParser)
    {
        _equationParser = equationParser;
    }

    public IEquationsReader CreateEquationsReader(string? equationsSourcePath)
    {
        if (string.IsNullOrEmpty(equationsSourcePath))
            throw new ArgumentNullException(nameof(equationsSourcePath));

        if (equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            throw new ArgumentException($"Параметр {nameof(equationsSourcePath)} не является путем к файлу.");

        return new FileEquationsReader(equationsSourcePath, _equationParser);
    }
}