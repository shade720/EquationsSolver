using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Readers;
public class FileEquationsReader : IEquationsReader
{
    private readonly IEquationParser _equationParser;
    private readonly string _filename;

    public FileEquationsReader(
        string filename,
        IEquationParser equationParser)
    {
        _filename = filename;
        _equationParser = equationParser;
    }

    public IEnumerable<Equation> Read()
    {
        Console.WriteLine("Осуществляется ввод уравнений из файла...");
        Console.WriteLine();

        using var sr = new StreamReader(_filename);
        var linesCounter = 0;
        while (!sr.EndOfStream)
        {
            linesCounter++;
            var coefficientLine = sr.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                Console.WriteLine($"Строка №{linesCounter} содержит ошибку. Уравнение будет пропущено.");
                continue;
            }

            Equation parsedEquation;
            try
            {
                parsedEquation = _equationParser.Parse(coefficientLine);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Строка {linesCounter}. Ошибка парсинга уравнения '{e.Message}'. Уравнение будет пропущено.");
                continue;
            }

            Console.WriteLine($"Получено уравнение {parsedEquation}.");
            yield return parsedEquation;
            Console.WriteLine();
        }
    }
}