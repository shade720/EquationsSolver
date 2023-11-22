using CommandLine;
using EquationsSolver.Application;
using EquationsSolver.Application.Readers;
using EquationsSolver.Application.Solvers;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EquationsSolver.ConsoleUI;

public static class Program
{
    public static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var options))
            return;

        var serviceProvider = new ServiceCollection()
            .AddSingleton(options)
            .AddTransient<IEquationParser, EquationParser>()
            .AddTransient<IEquationReaderFactory, ConsoleEquationReaderFactory>()
            .AddTransient<IEquationSolver, QuadraticEquationSolver>()
            .AddTransient<ISolvingResultsPresenter, ConsoleSolvingResultsPresenter>()
            .AddTransient<App>()
            .BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<App>();
        app.Start();
    }

    private static bool TryParseArgs(string[] args, out SolverOptions options)
    {
        var parser = new Parser(with =>
        {
            with.CaseInsensitiveEnumValues = true;
            with.HelpWriter = Console.Error;
        });

        var parserResult = parser.ParseArguments<ConsoleSolverOptions>(args);

        options = new SolverOptions
        {
            ThreadsNumber = parserResult.Value?.ThreadsNumber,
            EquationsFileName = parserResult.Value?.EquationsFileName
        };

        return parserResult.Tag == ParserResultType.Parsed;
    }
}