using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Readers;
public class ConsoleEquationsReader : IEquationsReader
{
    private readonly IEquationParser _equationParser;

    public ConsoleEquationsReader(IEquationParser equationParser)
    {
        _equationParser = equationParser;
    }

    public IEnumerable<Equation> Read()
    {
        Console.WriteLine("Работа осуществляется в формате: ввод -> решение -> ввод...");
        Console.WriteLine("Вводите коэффициенты уравнения через пробел в одной строке (значения с плавающей запятой вводятся через '.'). Например: 2 0 1.5 ....");

        var exitKey = default(ConsoleKey);
        while (exitKey != ConsoleKey.Escape)
        {
            Console.Write("Введите коэффициенты: ");
            var line = Console.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                Console.WriteLine("Коэффициенты введены неверно");
                continue;
            }

            var parsedEquation = _equationParser.Parse(line);

            if (parsedEquation is not null)
            {
                Console.WriteLine($"Получено уравнение {parsedEquation}");
                yield return parsedEquation;
            }
            else
                Console.WriteLine("Уравнение будет пропущено.");

            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить. Нажмите клавишу Esc, чтобы остановить выполение.");
            exitKey = Console.ReadKey().Key;
        }
    }
}