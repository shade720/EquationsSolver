using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EquationsSolver.Application.UnitTests;

public class FakeFileEquationsReader : EquationsReaderBase
{
    private readonly string _fileContent;

    public FakeFileEquationsReader(
        string fileContent,
        ILogger logger, 
        IEquationParser equationParser) 
        : base(logger, equationParser)
    {
        _fileContent = fileContent;
    }

    protected override Stream OpenEquationsSource()
    {
        var fakeFileBytes = Encoding.UTF8.GetBytes(_fileContent);
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);
        return fakeMemoryStream;
    }
}