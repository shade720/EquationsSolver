using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Readers;
public sealed class FileEquationsReader : IEquationsReader
{
    private readonly string _filename;

    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;

    public FileEquationsReader(
        string filename,
        ILogger logger,
        IEquationParser equationParser)
    {
        if (string.IsNullOrEmpty(filename))
            throw new ArgumentNullException(nameof(filename));
        _filename = filename;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _equationParser = equationParser ?? throw new ArgumentNullException(nameof(equationParser));
    }

    public IEnumerable<Equation> Read()
    {
        _logger.LogInformation("Осуществляется ввод уравнений из файла...\r\n");

        using var sr = new StreamReader(_filename);
        var linesCounter = 0;
        while (!sr.EndOfStream)
        {
            linesCounter++;
            var coefficientLine = sr.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                _logger.LogError("Строка №{0} содержит ошибку. Уравнение будет пропущено.", linesCounter);
                continue;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = _equationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                _logger.LogError("Строка {0}. Ошибка парсинга уравнения '{1}'. Уравнение будет пропущено.", linesCounter, e.Message);
                continue;
            }

            _logger.LogInformation("Получено уравнение {0}.", parsedEquation);
            yield return parsedEquation;
        }
    }
}