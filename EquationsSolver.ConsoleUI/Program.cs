using CommandLine;
using EquationsSolver.Application;
using EquationsSolver.Application.Readers;
using EquationsSolver.Application.Solvers;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EquationsSolver.ConsoleUI;

public class Program
{
    public static void Main(string[] args)
    {
        if (!TryParseArgs(args, out var options))
            return;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        // Регистрируем Serilog при создании Microsoft.Extensions.Logging.LoggerFactory
        using var loggerFactory = LoggerFactory.Create(
            builder => builder.AddSerilog(dispose: true));
        // Создаем экземпляр ILogger при помощи фабрики
        var logger = loggerFactory.CreateLogger<Program>();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger>(logger)
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