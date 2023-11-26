using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Factories;

public sealed class DefaultEquationReaderFactory : IEquationReaderFactory
{
    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;
    private readonly IStreamReaderFactory _streamReaderFactory;

    public DefaultEquationReaderFactory(
        ILogger logger,
        IEquationParser equationParser, 
        IStreamReaderFactory streamReaderFactory)
    {
        _logger = logger;
        _equationParser = equationParser;
        _streamReaderFactory = streamReaderFactory;
    }

    public IEquationsReader CreateEquationsReader(string? equationsSourcePath)
    {
        if (string.IsNullOrEmpty(equationsSourcePath))
            throw new ArgumentNullException(nameof(equationsSourcePath));

        if (equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            throw new ArgumentException($"Параметр {nameof(equationsSourcePath)} не является путем к файлу.");

        return new FileEquationsReader(equationsSourcePath, _streamReaderFactory, _logger, _equationParser);
    }
}