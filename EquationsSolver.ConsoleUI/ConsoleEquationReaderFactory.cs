using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.ConsoleUI;

public sealed class ConsoleEquationReaderFactory : IEquationReaderFactory
{
    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;

    public ConsoleEquationReaderFactory(
        ILogger logger,
        IEquationParser equationParser)
    {
        _logger = logger;
        _equationParser = equationParser;
    }

    public EquationsReaderBase CreateEquationsReader(string? equationsSourcePath)
    {
        if (!string.IsNullOrEmpty(equationsSourcePath) && equationsSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            return new FileEquationsReader(equationsSourcePath, _logger, _equationParser);
        
        return new ConsoleEquationsReader(_logger, _equationParser);
    }
}