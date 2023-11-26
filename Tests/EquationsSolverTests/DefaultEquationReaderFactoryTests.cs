using EquationsSolver.Application.Factories;
using EquationsSolver.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EquationsSolver.Application.UnitTests;

public class DefaultEquationReaderFactoryTests
{
    private readonly DefaultEquationReaderFactory _sut;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IEquationParser> _equationParserMock;

    public DefaultEquationReaderFactoryTests()
    {
        _loggerMock = new Mock<ILogger>();
        _equationParserMock = new Mock<IEquationParser>();
        _sut = new DefaultEquationReaderFactory(_loggerMock.Object, _equationParserMock.Object);
    }

    [Fact]
    public void CreateEquationsReader_CorrectSourcePath_SuccessfullyCreated()
    {
        // Arrange
        const string testSourcePath = @".\TestFiles\equations.txt";

        // Act
        var actualReader = _sut.CreateEquationsReader(testSourcePath);

        // Assert
        Assert.NotNull(actualReader);
    }

    [Fact]
    public void CreateEquationsReader_IncorrectSourcePath_ArgumentException()
    {
        // Arrange
        const string incorrectSourcePath = @".\TestFiles|equations.txt";
        var emptySourcePath = string.Empty;

        // Act, Assert
        Assert.Throws<ArgumentException>(() => _sut.CreateEquationsReader(incorrectSourcePath));
        Assert.Throws<ArgumentNullException>(() => _sut.CreateEquationsReader(emptySourcePath));
    }

    [Fact]
    public void CreateEquationsReader_NullEquationParser_ArgumentException()
    {
        // Arrange
        const string testSourcePath = @".\TestFiles\empty_file.txt";
        var localSut = new DefaultEquationReaderFactory(_loggerMock.Object, null);

        // Act, Assert
        Assert.Throws<ArgumentNullException>(() => localSut.CreateEquationsReader(testSourcePath));
    }

    [Fact]
    public void CreateEquationsReader_NullLogger_ArgumentException()
    {
        // Arrange
        const string testSourcePath = @".\TestFiles\empty_file.txt";
        var localSut = new DefaultEquationReaderFactory(null, _equationParserMock.Object);

        // Act, Assert
        Assert.Throws<ArgumentNullException>(() => localSut.CreateEquationsReader(testSourcePath));
    }
}