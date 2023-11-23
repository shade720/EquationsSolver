using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace EquationsSolver.Application.Tests;

public class AppTests : IClassFixture<TestDataFixture>
{
    private App _sut;
    
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IEquationReaderFactory> _equationReaderFactoryMock = new();
    private readonly Mock<IEquationSolver> _equationSolverMock = new();
    private readonly Mock<ISolvingResultsPresenter> _solvingResultsPresenterMock = new();
    private readonly Mock<IEquationsReader> _equationsReaderMock = new();

    private readonly TestDataFixture _testDataFixture;

    public AppTests(TestDataFixture testDataFixture)
    {
        _testDataFixture = testDataFixture;
    }

    [Fact]
    public void Start_SequentialExecution()
    {
        // Arrange
        const string expectedLogMessage = "Используется последовательный режим\r\n";

        var options = new SolverOptions
        {
            EquationsFileName = TestDataFixture.FakeEquationSourcePath,
            ThreadsNumber = null
        };
        
        _sut = new App(
            options, 
            _loggerMock.Object, 
            _equationReaderFactoryMock.Object, 
            _equationSolverMock.Object,
            _solvingResultsPresenterMock.Object);

        _equationReaderFactoryMock
            .Setup(x => x.CreateEquationsReader(TestDataFixture.FakeEquationSourcePath))
            .Returns(_equationsReaderMock.Object);
        _equationsReaderMock
            .Setup(x => x.Read())
            .Returns(_testDataFixture.TestEquations);
        _equationSolverMock
            .Setup(x => x.Solve(_testDataFixture.TestEquations[0]))
            .Returns(_testDataFixture.TestResults[0]);
        _equationSolverMock
            .Setup(x => x.Solve(_testDataFixture.TestEquations[1]))
            .Returns(_testDataFixture.TestResults[1]);

        // Act
        _sut.Start();

        // Assert
        _equationReaderFactoryMock.Verify(x => x.CreateEquationsReader(TestDataFixture.FakeEquationSourcePath), Times.Once);
        _equationsReaderMock.Verify(x => x.Read(), Times.Once);
        _equationSolverMock.Verify(x => x.Solve(_testDataFixture.TestEquations[0]), Times.Once);
        _equationSolverMock.Verify(x => x.Solve(_testDataFixture.TestEquations[1]), Times.Once);
        _solvingResultsPresenterMock.Verify(x => x.ShowResults(_testDataFixture.TestResults[0]), Times.Once);
        _solvingResultsPresenterMock.Verify(x => x.ShowResults(_testDataFixture.TestResults[1]), Times.Once);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedLogMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Start_ParallelExecution()
    {
        // Arrange
        const string expectedLogMessage = "Используется параллельный режим\r\n";

        var options = new SolverOptions
        {
            EquationsFileName = TestDataFixture.FakeEquationSourcePath,
            ThreadsNumber = 2
        };

        _sut = new App(
            options,
            _loggerMock.Object,
            _equationReaderFactoryMock.Object,
            _equationSolverMock.Object,
            _solvingResultsPresenterMock.Object);

        _equationReaderFactoryMock
            .Setup(x => x.CreateEquationsReader(TestDataFixture.FakeEquationSourcePath))
            .Returns(_equationsReaderMock.Object);
        _equationsReaderMock
            .Setup(x => x.Read())
            .Returns(_testDataFixture.TestEquations);
        _equationSolverMock
            .Setup(x => x.Solve(_testDataFixture.TestEquations[0]))
            .Returns(_testDataFixture.TestResults[0]);
        _equationSolverMock
            .Setup(x => x.Solve(_testDataFixture.TestEquations[1]))
            .Returns(_testDataFixture.TestResults[1]);

        // Act
        _sut.Start();

        // Assert
        _equationReaderFactoryMock.Verify(x => x.CreateEquationsReader(TestDataFixture.FakeEquationSourcePath), Times.Once);
        _equationsReaderMock.Verify(x => x.Read(), Times.Once);
        _equationSolverMock.Verify(x => x.Solve(_testDataFixture.TestEquations[0]), Times.Once);
        _equationSolverMock.Verify(x => x.Solve(_testDataFixture.TestEquations[1]), Times.Once);
        _solvingResultsPresenterMock.Verify(x => x.ShowResults(_testDataFixture.TestResults[0]), Times.Once);
        _solvingResultsPresenterMock.Verify(x => x.ShowResults(_testDataFixture.TestResults[1]), Times.Once);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedLogMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}

public class TestDataFixture
{
    public const string FakeEquationSourcePath = @"TestFiles\test_equations.txt";

    public readonly Equation[] TestEquations = {
        new(new[] { 1.0, 0, 1.0 }),
        new(new[] { 2.0, 5.0, -3.5 })
    };

    public readonly EquationSolvingResult[] TestResults = {
        new()
        {
            IsSolvedSuccessful = true,
            OriginalEquation = new Equation(new[] { 1.0, 0, 1.0 }),
            Roots = new[] { 2.2801, -12.2801 }
        },
        new()
        {
            IsSolvedSuccessful = true,
            OriginalEquation = new Equation(new[] { 2.0, 5.0, -3.5 }),
            Roots = new[] { 3.0 }
        }
    };
}