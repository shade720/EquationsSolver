namespace EquationsSolver.Domain.Abstractions;

public interface IEquationReaderFactory
{
    public EquationsReaderBase CreateEquationsReader(string? equationsSourcePath);
}