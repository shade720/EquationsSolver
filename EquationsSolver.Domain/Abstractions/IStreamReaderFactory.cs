namespace EquationsSolver.Domain.Abstractions;

public interface IStreamReaderFactory
{
    public StreamReader GetStreamReader(string filename);
}