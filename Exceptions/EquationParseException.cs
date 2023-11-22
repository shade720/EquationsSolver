namespace EquationsSolver.Exceptions;

public class EquationParseException : Exception
{
    public EquationParseException() { }
    public EquationParseException(string message) : base(message) { }
}