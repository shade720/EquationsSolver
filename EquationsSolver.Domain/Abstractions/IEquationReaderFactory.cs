namespace EquationsSolver.Domain.Abstractions;

public interface IEquationReaderFactory
{
    public IEquationsReader CreateEquationsReader(string? equationsSourcePath);
}