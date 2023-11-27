using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Domain.Abstractions;
public abstract class EquationsReaderBase
{
    private readonly ILogger _logger;
    private readonly IEquationParser _equationParser;

    protected EquationsReaderBase(
        ILogger logger,
        IEquationParser equationParser)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _equationParser = equationParser ?? throw new ArgumentNullException(nameof(equationParser));
    }

    public IEnumerable<Equation> Read()
    {
        using var sr = new StreamReader(OpenEquationsSource());
        var linesCounter = 0;
        while (true)
        {
            linesCounter++;

            _logger.LogInformation("Введите коэффициенты: ");
            var coefficientLine = sr.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                _logger.LogError("Полученная строка №{0} пустая. Завершаем чтение...", linesCounter);
                yield break;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = _equationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                _logger.LogError("Строка {0}. Ошибка парсинга уравнения '{1}'. Уравнение будет пропущено.", 
                    linesCounter, e.Message);
                continue;
            }

            _logger.LogInformation("Получено уравнение {0}.", parsedEquation);
            yield return parsedEquation;
        } 
    }

    protected abstract Stream OpenEquationsSource();
}