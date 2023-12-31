﻿using EquationsSolver.Application.Readers;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Exceptions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace EquationsSolver.Application.UnitTests;

public class FileEquationsReaderTests
{
    private readonly FileEquationsReader _sut;
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IEquationParser> _equationParserMock = new();
    private readonly Mock<IStreamReaderFactory> _readerFactoryMock = new();
    private const string FakeFileName = "equitation.txt";

    public FileEquationsReaderTests()
    {
        _sut = new FileEquationsReader(FakeFileName, _readerFactoryMock.Object, _loggerMock.Object, _equationParserMock.Object);
    }

    [Fact]
    public void Read_ThreeLines_ThreeEquations()
    {
        // Arrange
        const string line1 = "1 0 1";
        const string line2 = "2 5 -3.5";
        const string line3 = "1 1 1";

        var expectedEquations = new[]
        {
            new Equation(new[] { 1.0, 0, 1.0 }),
            new Equation(new[] { 2.0, 5.0, -3.5 }),
            new Equation(new[] { 1.0, 1.0, 1.0 })
        };

        var fakeFileBytes = Encoding.UTF8.GetBytes(string.Join("\r\n", line1, line2, line3));
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        _equationParserMock
            .Setup(x => x.Parse(line1))
            .Returns(expectedEquations[0]);
        _equationParserMock
            .Setup(x => x.Parse(line2))
            .Returns(expectedEquations[1]);
        _equationParserMock
            .Setup(x => x.Parse(line3))
            .Returns(expectedEquations[2]);
        _readerFactoryMock
            .Setup(x => x.GetStreamReader(FakeFileName))
            .Returns(new StreamReader(fakeMemoryStream));

        // Act
        var actuallyReadEquations = _sut.Read().ToArray();

        // Assert
        Assert.Equal(expectedEquations.Length, actuallyReadEquations.Length);
        Assert.Equal(expectedEquations[0].Coefficients, actuallyReadEquations[0].Coefficients);
        Assert.Equal(expectedEquations[1].Coefficients, actuallyReadEquations[1].Coefficients);
        Assert.Equal(expectedEquations[2].Coefficients, actuallyReadEquations[2].Coefficients);

        _equationParserMock.Verify(x => x.Parse(line1), Times.Once);
        _equationParserMock.Verify(x => x.Parse(line2), Times.Once);
        _equationParserMock.Verify(x => x.Parse(line3), Times.Once);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Fact]
    public void Read_TwoBadLinesOutOfThree_OneEquationTwoErrors()
    {
        // Arrange
        const string line1 = "1 0 1";
        const string line2 = "2 5 -3.5";
        const string line3 = "1 1 1";
        const string errorMessage1 =
            "Строка 2. Ошибка парсинга уравнения 'Exception of type 'EquationsSolver.Domain.Exceptions.EquationParseException' was thrown.'. Уравнение будет пропущено.";
        const string errorMessage2 =
            "Строка 3. Ошибка парсинга уравнения 'Exception of type 'EquationsSolver.Domain.Exceptions.EquationParseException' was thrown.'. Уравнение будет пропущено.";

        var expectedEquations = new[]
        {
            new Equation(new[] { 1.0, 0, 1.0 })
        };

        var fakeFileBytes = Encoding.UTF8.GetBytes(string.Join("\r\n", line1, line2, line3));
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        _equationParserMock
            .Setup(x => x.Parse(line1))
            .Returns(expectedEquations[0]);
        _equationParserMock
            .Setup(x => x.Parse(line2))
            .Throws<EquationParseException>();
        _equationParserMock
            .Setup(x => x.Parse(line3))
            .Throws<EquationParseException>();
        _readerFactoryMock
            .Setup(x => x.GetStreamReader(FakeFileName))
            .Returns(new StreamReader(fakeMemoryStream));

        // Act
        var actuallyReadEquations = _sut.Read().ToArray();

        // Assert
        Assert.Single(actuallyReadEquations);
        Assert.Equal(expectedEquations[0].Coefficients, actuallyReadEquations[0].Coefficients);

        _equationParserMock.Verify(x => x.Parse(line1), Times.Once);
        _equationParserMock.Verify(x => x.Parse(line2), Times.Once);
        _equationParserMock.Verify(x => x.Parse(line3), Times.Once);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == errorMessage1 && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == errorMessage2 && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Read_EmptyFile_NoEquationsNoExceptions()
    {
        // Arrange
        const string errorMessage1 = "Строка №1 содержит ошибку. Уравнение будет пропущено.";
        var fakeFileBytes = Encoding.UTF8.GetBytes("\r\n");
        var fakeMemoryStream = new MemoryStream(fakeFileBytes);

        _readerFactoryMock
            .Setup(x => x.GetStreamReader(FakeFileName))
            .Returns(new StreamReader(fakeMemoryStream));

        // Act
        var actuallyReadEquations = _sut.Read().ToArray();

        // Assert
        Assert.Empty(actuallyReadEquations);

        _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == errorMessage1 && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}