using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.ConsoleUI;

public sealed class ConsoleEquationReaderFactory : IEquationReaderFactory
{
    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;
    private readonly IStreamReaderFactory _streamReaderFactory;

    public ConsoleEquationReaderFactory(
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
        if (!string.IsNullOrEmpty(equationsSourcePath) && equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            return new FileEquationsReader(equationsSourcePath, _streamReaderFactory, _logger, _equationParser);
        
        return new ConsoleEquationsReader(_equationParser);
    }
}