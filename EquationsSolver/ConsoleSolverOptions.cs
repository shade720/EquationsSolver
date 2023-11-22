using CommandLine;

namespace EquationsSolver.ConsoleUI;

public class ConsoleSolverOptions
{
    [Option('f', "filename", Required = false, HelpText = "Путь к файлу, в котором записаны уравнения.")]
    public string? EquationsFileName { get; set; }

    [Option('p', "parallel", Required = false,
        HelpText = "Вычисления будут производиться параллельно в указаном числе потоков. Параметр не доступен для консольного ввода.")]
    public int? ThreadsNumber { get; set; }
}