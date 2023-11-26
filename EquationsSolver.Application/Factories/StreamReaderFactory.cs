using EquationsSolver.Domain.Abstractions;

namespace EquationsSolver.Application.Factories;

public class StreamReaderFactory : IStreamReaderFactory
{
    public StreamReader GetStreamReader(string filename) => new(filename);
}