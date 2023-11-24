using EquationsSolver.Application.Solvers;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.Application.Tests;

public class QuadraticEquationSolverTests
{
    private readonly QuadraticEquationSolver _sut = new();
    private const double Epsilon = 0.0001;

    [Fact]
    public void Solve_EquationWithTwoRoots_ResultWithTwoRoots()
    {
        // Arrange
        var testEquation = new Equation(new[] { 2, 5, -3.5 });
        var expectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = true,
            Roots = new[] { 2.2801, -12.2801 }
        };

        // Act
        var actualSolvingResult = _sut.Solve(testEquation);

        // Assert
        Assert.True(actualSolvingResult.IsSolvedSuccessful);
        Assert.Equal(expectedSolvingResult.OriginalEquation, actualSolvingResult.OriginalEquation);
        Assert.Equal(expectedSolvingResult.Roots, actualSolvingResult.Roots, (e,a) => Math.Abs(e - a) < Epsilon);
    }

    [Fact]
    public void Solve_EquationWithOneRoot_ResultWithOneRoot()
    {
        // Arrange
        var testEquation = new Equation(new[] { 1, -6, 9.0 });
        var expectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = true,
            Roots = new[] { 3.0 }
        };

        // Act
        var actualSolvingResult = _sut.Solve(testEquation);

        // Assert
        Assert.True(actualSolvingResult.IsSolvedSuccessful);
        Assert.Equal(expectedSolvingResult.OriginalEquation, actualSolvingResult.OriginalEquation);

        Assert.Single(actualSolvingResult.Roots);
        Assert.Equal(expectedSolvingResult.Roots.FirstOrDefault(), actualSolvingResult.Roots.FirstOrDefault(), (e, a) => Math.Abs(e - a) < Epsilon);
    }

    [Fact]
    public void Solve_EquationWithNoRoot_ResultWithNoRoot()
    {
        // Arrange
        var testEquation = new Equation(new[] { 1, 0, 1.0 });
        var expectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = true,
            Roots = Array.Empty<double>()
        };

        // Act
        var actualSolvingResult = _sut.Solve(testEquation);

        // Assert
        Assert.True(actualSolvingResult.IsSolvedSuccessful);
        Assert.Equal(expectedSolvingResult.OriginalEquation, actualSolvingResult.OriginalEquation);

        Assert.Empty(actualSolvingResult.Roots);
    }

    [Fact]
    public void Solve_WrongDegreeEquation_UnsuccessfulResult()
    {
        // Arrange
        var higherDegreeEquation = new Equation(new[] { 1, 0, 1.0, 3 });
        var lowerDegreeEquation = new Equation(new[] { 1.0, 0 });

        var higherDegreeExpectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = higherDegreeEquation,
            IsSolvedSuccessful = false,
            Roots = Array.Empty<double>()
        };

        var lowerDegreeExpectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = lowerDegreeEquation,
            IsSolvedSuccessful = false,
            Roots = Array.Empty<double>()
        };

        // Act
        var actualHigherDegreeEquationSolvingResult = _sut.Solve(higherDegreeEquation);
        var actualLowerDegreeEquationSolvingResult = _sut.Solve(lowerDegreeEquation);

        // Assert
        Assert.False(actualHigherDegreeEquationSolvingResult.IsSolvedSuccessful);
        Assert.False(actualLowerDegreeEquationSolvingResult.IsSolvedSuccessful);

        Assert.Empty(actualHigherDegreeEquationSolvingResult.Roots);
        Assert.Empty(actualLowerDegreeEquationSolvingResult.Roots);

        Assert.Equal(
            actualHigherDegreeEquationSolvingResult.OriginalEquation, 
            higherDegreeExpectedSolvingResult.OriginalEquation);
        Assert.Equal(
            actualLowerDegreeEquationSolvingResult.OriginalEquation,
            lowerDegreeExpectedSolvingResult.OriginalEquation);
    }

    [Fact]
    public void Solve_ZeroLeadingCoefficient_UnsuccessfulResult()
    {
        // Arrange
        var zeroLeadingCoefficientEquation = new Equation(new[] { 0, -6.0, 5 });
        var expectedSolvingResult = new EquationSolvingResult
        {
            OriginalEquation = zeroLeadingCoefficientEquation,
            IsSolvedSuccessful = false,
            Roots = Array.Empty<double>()
        };

        // Act
        var actualSolvingResult = _sut.Solve(zeroLeadingCoefficientEquation);

        // Assert
        Assert.False(actualSolvingResult.IsSolvedSuccessful);
        Assert.Empty(actualSolvingResult.Roots);
        Assert.Equal(actualSolvingResult.OriginalEquation, expectedSolvingResult.OriginalEquation);
    }
}