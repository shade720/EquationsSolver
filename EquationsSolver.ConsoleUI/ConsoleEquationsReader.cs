using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.ConsoleUI;

public sealed class ConsoleEquationsReader : EquationsReaderBase
{
    public ConsoleEquationsReader(
        ILogger logger, 
        IEquationParser equationParser) 
        : base(logger, equationParser) { }

    protected override Stream OpenEquationsSource()
    {
        return Console.OpenStandardInput();
    }
}