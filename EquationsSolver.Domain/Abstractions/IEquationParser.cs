using EquationsSolver.Domain.Models;

namespace EquationsSolver.Domain.Abstractions;

public interface IEquationParser
{
    public Equation Parse(string coefficientLine);
}