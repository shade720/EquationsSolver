using EquationsSolver.Models;

namespace EquationsSolver.Abstractions;

public interface IEquationsReader
{
    public IEnumerable<Equation> Read();
}