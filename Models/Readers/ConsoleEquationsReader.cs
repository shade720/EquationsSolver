using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Readers;
public class ConsoleEquationsReader : IEquationsReader
{
    private readonly IEquationParser _equationParser;

    public ConsoleEquationsReader(
        IEquationParser equationParser)
    {
        _equationParser = equationParser;
    }

    public IEnumerable<Equation> Read()
    {
        Console.WriteLine("Осуществляется ввод уравнений с консоли (в формате: ввод -> решение -> ввод...).");
        Console.WriteLine("Вводите коэффициенты уравнения через пробел в одной строке (значения с плавающей запятой вводятся через '.').");
        Console.WriteLine("Например: 2 0 1.5");

        var exitKey = default(ConsoleKey);

        while (exitKey != ConsoleKey.Escape)
        {
            Console.WriteLine();
            Console.Write("Введите коэффициенты: ");
            var coefficientLine = Console.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                Console.WriteLine("Коэффициенты введены неверно");
                continue;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = _equationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка парсинга уравнения '{e.Message}'. Уравнение будет пропущено.");
                continue;
            }

            Console.WriteLine($"Получено уравнение {parsedEquation}");
            yield return parsedEquation;

            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить. Нажмите клавишу Esc, чтобы остановить выполение.");
            exitKey = Console.ReadKey().Key;
        }
    }
}