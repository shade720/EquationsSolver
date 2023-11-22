namespace EquationsSolver.Abstractions;

public interface IEquationReaderFactory
{
    public IEquationsReader CreateEquationsReader(string? equationsPath);
}