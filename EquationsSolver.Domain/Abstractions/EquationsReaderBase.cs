using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Domain.Abstractions;
public abstract class EquationsReaderBase
{
    protected readonly ILogger Logger;
    protected readonly IEquationParser EquationParser;

    protected EquationsReaderBase(
        ILogger logger,
        IEquationParser equationParser)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        EquationParser = equationParser ?? throw new ArgumentNullException(nameof(equationParser));
    }

    public virtual IEnumerable<Equation> Read()
    {
        using var sr = new StreamReader(OpenEquationsSource());
        var linesCounter = 0;
        while (!sr.EndOfStream)
        {
            linesCounter++;

            Logger.LogInformation("Введите коэффициенты: ");
            var coefficientLine = sr.ReadLine();
            
            if (string.IsNullOrEmpty(coefficientLine))
            {
                Logger.LogError("Строка №{0} содержит ошибку. Уравнение будет пропущено.", linesCounter);
                continue;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = EquationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                Logger.LogError("Строка {0}. Ошибка парсинга уравнения '{1}'. Уравнение будет пропущено.", linesCounter, e.Message);
                continue;
            }

            Logger.LogInformation("Получено уравнение {0}.", parsedEquation);
            yield return parsedEquation;
        }
    }

    protected abstract Stream OpenEquationsSource();
}