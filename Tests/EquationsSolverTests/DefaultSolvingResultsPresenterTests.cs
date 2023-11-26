using EquationsSolver.Application.Presenters;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace EquationsSolver.Application.UnitTests;

public class DefaultSolvingResultsPresenterTests
{
    private readonly DefaultSolvingResultsPresenter _sut;
    private readonly Mock<ILogger> _loggerMock;

    public DefaultSolvingResultsPresenterTests()
    {
        _loggerMock = new Mock<ILogger>();
        _sut = new DefaultSolvingResultsPresenter(_loggerMock.Object);
    }

    [Fact]
    public void ShowingResults_ResultWithTwoRoots_LogMessageWithTwoRoots()
    {
        // Arrange
        var testEquation = new Equation(new[] { 2, 5, -3.5 });
        var testSolvingResults = new EquationSolvingResult
        {
            IsSolvedSuccessful = true,
            OriginalEquation = testEquation,
            Roots = new[] { 2.2801, -12.2801 }
        };
        const string expectedMessage = "Уравнение 2x^2 + (5)x^1 + (-3,5). Действительных корней - 2. Корни: 2,2801; -12,2801";

        // Act
        _sut.ShowResults(testSolvingResults);

        // Assert
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void ShowingResults_ResultWithOneRoot_LogMessageWithOneRoots()
    {
        // Arrange
        var testEquation = new Equation(new[] { 1, -6, 9.0 });
        var testSolvingResults = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = true,
            Roots = new[] { 3.0 }
        };
        const string expectedMessage = "Уравнение 1x^2 + (-6)x^1 + (9) имеет 1 действительный корень - 3";

        // Act
        _sut.ShowResults(testSolvingResults);

        // Assert
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void ShowingResults_ResultWithNoRoots_LogMessageWithNoRoots()
    {
        // Arrange
        var testEquation = new Equation(new[] { 1, 0, 1.0 });
        var testSolvingResults = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = true,
            Roots = Array.Empty<double>()
        };
        const string expectedMessage = "Уравнение 1x^2 + (0)x^1 + (1) не имеет действтельных корней";

        // Act
        _sut.ShowResults(testSolvingResults);

        // Assert
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void ShowingResults_UnsuccessfulResult_LogMessageAboutUnsuccessfulResult()
    {
        // Arrange
        var testEquation = new Equation(new[] { 1.0, 0 });
        var testSolvingResults = new EquationSolvingResult
        {
            OriginalEquation = testEquation,
            IsSolvedSuccessful = false,
            Roots = Array.Empty<double>()
        };
        var expectedMessage = $"Не удалось решить уравнение '{testSolvingResults.OriginalEquation}'. Возможно уравнение имеет неверную степень.";

        // Act
        _sut.ShowResults(testSolvingResults);

        // Assert
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}