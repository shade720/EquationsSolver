using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Exceptions;

namespace EquationsSolver.Application.Tests;

public class EquationParserTests
{
    private readonly EquationParser _sut = new();

    [Fact]
    public void Parse_ThreeCoefficients_Success()
    {
        // Arrange
        const int coefficient1 = 1;
        const int coefficient2 = 0;
        const int coefficient3 = 2;
        var coefficientsLine = $"{coefficient1} {coefficient2} {coefficient3}";
        const int expectedEquationCoefficientsNumber = 3;

        // Act
        var actualEquation = _sut.Parse(coefficientsLine);

        // Assert
        Assert.Equal(expectedEquationCoefficientsNumber, actualEquation.Coefficients.Count);
        Assert.Equal(coefficient1, actualEquation.Coefficients.ElementAt(0));
        Assert.Equal(coefficient2, actualEquation.Coefficients.ElementAt(1));
        Assert.Equal(coefficient3, actualEquation.Coefficients.ElementAt(2));
    }

    [Fact]
    public void Parse_EmptyLine_ThrowArgumentNullException()
    {
        // Arrange
        var coefficientsLine = string.Empty;

        // Act, Assert
        Assert.Throws<ArgumentNullException>(() => _sut.Parse(coefficientsLine));
    }

    [Fact]
    public void Parse_BadCoefficient_ThrowEquationParseException()
    {
        // Arrange
        const string badCoefficient = "fd";
        const string coefficientsLine = $"1 {badCoefficient} 2";

        // Act, Assert
        Assert.Throws<EquationParseException>(() => _sut.Parse(coefficientsLine));
    }

    [Fact]
    public void Parse_CoefficientWithComma_ThrowEquationParseException()
    {
        // Arrange
        const string coefficientWithComma = "1,5";
        const string coefficientsLine = $"1 {coefficientWithComma} 2";

        // Act, Assert
        Assert.Throws<EquationParseException>(() => _sut.Parse(coefficientsLine));
    }

    [Fact]
    public void Parse_OneCoefficient_ThrowArgumentException()
    {
        // Arrange
        const string coefficientsLine = "1";

        // Act, Assert
        Assert.Throws<ArgumentException>(() => _sut.Parse(coefficientsLine));
    }
}