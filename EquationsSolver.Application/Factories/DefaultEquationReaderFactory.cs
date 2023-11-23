using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Factories;

public sealed class DefaultEquationReaderFactory : IEquationReaderFactory
{
    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;

    public DefaultEquationReaderFactory(
        ILogger logger,
        IEquationParser equationParser)
    {
        _logger = logger;
        _equationParser = equationParser;
    }

    public IEquationsReader CreateEquationsReader(string? equationsSourcePath)
    {
        if (string.IsNullOrEmpty(equationsSourcePath))
            throw new ArgumentNullException(nameof(equationsSourcePath));

        if (equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            throw new ArgumentException($"Параметр {nameof(equationsSourcePath)} не является путем к файлу.");

        return new FileEquationsReader(equationsSourcePath, _logger, _equationParser);
    }
}