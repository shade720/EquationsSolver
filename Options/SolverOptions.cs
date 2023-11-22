using CommandLine;

namespace EquationsSolver.Options;

public class SolverOptions
{
    [Option('f', "filename", Required = false, HelpText = "Путь к файлу, в котором записаны уравнения.")]
    public string? EquationsFileName { get; set; }
}