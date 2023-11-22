using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Readers;
public class FileEquationsReader : IEquationsReader
{
    private readonly IEquationParser _equationParser;
    private readonly string _filename;

    public FileEquationsReader(string filename, IEquationParser equationParser)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException($"Файл с уравнениями не найден по пути: {filename}");
        _filename = filename;
        _equationParser = equationParser;
    }

    public IEnumerable<Equation> Read()
    {
        Console.WriteLine("Производится чтение уравнений из файла...");

        using var sr = new StreamReader(_filename);
        while (!sr.EndOfStream)
        {
            var coefficientLine = sr.ReadLine();

            if (string.IsNullOrEmpty(coefficientLine))
            {
                Console.WriteLine($"Строку {coefficientLine} распарсить не удалось. Уравнение будет пропущено.");
                continue;
            }

            var parsedEquation = _equationParser.Parse(coefficientLine);
            if (parsedEquation is not null)
            {
                Console.WriteLine($"Получено уравнение {parsedEquation}");
                yield return parsedEquation;
            }
            else
                Console.WriteLine($"Строку {coefficientLine} распарсить не удалось. Уравнение будет пропущено.");
        }
    }
}