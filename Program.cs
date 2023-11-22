using CommandLine;
using EquationsSolver;
using EquationsSolver.Abstractions;
using EquationsSolver.Factories;
using EquationsSolver.Models.Readers;
using EquationsSolver.Models.Solvers;
using EquationsSolver.Options;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var options))
            return;

        var serviceProvider = new ServiceCollection()
            .AddSingleton(options)
            .AddTransient<IEquationParser, EquationParser>()
            .AddTransient<IEquationReaderFactory, EquationReaderFactory>()
            .AddTransient<IEquationSolver, QuadraticEquationSolver>()
            .AddTransient<ISolvingResultsPresenterFactory, SolvingResultsPresenterFactory>()
            .AddTransient<Application>()
            .BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<Application>();
        app.Start();
    }

    private static bool TryParseArgs(string[] args, out SolverOptions options)
    {
        var parser = new Parser(with => {
            with.CaseInsensitiveEnumValues = true;
            with.HelpWriter = Console.Error;
        });

        var parserResult = parser.ParseArguments<SolverOptions>(args);

        options = parserResult.Value;

        return parserResult.Tag == ParserResultType.Parsed;
    }
}