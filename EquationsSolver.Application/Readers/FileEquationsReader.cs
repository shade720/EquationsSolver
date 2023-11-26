using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Readers;
public sealed class FileEquationsReader : EquationsReaderBase
{
    private readonly string _filename;

    public FileEquationsReader(
        string filename,
        ILogger logger,
        IEquationParser equationParser) 
        : base(logger, equationParser)
    {
        _filename = filename;
    }

    protected override Stream OpenEquationsSource()
    {
        return new FileStream(_filename, FileMode.Open, FileAccess.Read);
    }
}