using EquationsSolver.Domain.Models;

namespace EquationsSolver.Domain.Abstractions;

public interface IEquationsReader
{
    public IEnumerable<Equation> Read();
}