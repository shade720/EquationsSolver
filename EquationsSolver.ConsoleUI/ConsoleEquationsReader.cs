using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.ConsoleUI;

public sealed class ConsoleEquationsReader : EquationsReaderBase
{
    public ConsoleEquationsReader(
        ILogger logger, 
        IEquationParser equationParser) 
        : base(logger, equationParser) { }

    public override IEnumerable<Equation> Read()
    {
        Logger.LogInformation("Осуществляется ввод уравнений с консоли (в формате: ввод -> решение -> ввод...).");
        Logger.LogInformation("Вводите коэффициенты уравнения через пробел в одной строке (значения с плавающей запятой вводятся через '.').");
        Logger.LogInformation("Например: 2 0 1.5");

        var exitKey = default(ConsoleKey);

        while (exitKey != ConsoleKey.Escape)
        {
            Console.Write("\r\nВведите коэффициенты: ");
            var coefficientLine = Console.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                Logger.LogError("Коэффициенты введены неверно");
                continue;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = EquationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                Logger.LogError($"Ошибка парсинга уравнения '{e.Message}'. Уравнение будет пропущено.");
                continue;
            }

            Logger.LogInformation($"Получено уравнение {parsedEquation}");
            yield return parsedEquation;

            Logger.LogInformation("Нажмите любую клавишу, чтобы продолжить. Нажмите клавишу Esc, чтобы остановить выполение.");
            exitKey = Console.ReadKey().Key;
        }
    }

    protected override Stream OpenEquationsSource()
    {
        return Console.OpenStandardInput();
    }
}