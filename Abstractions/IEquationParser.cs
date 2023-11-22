using EquationsSolver.Models;

namespace EquationsSolver.Abstractions;

public interface IEquationParser
{
    public Equation Parse(string coefficientLine);
}