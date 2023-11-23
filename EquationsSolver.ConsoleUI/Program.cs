using CommandLine;
using EquationsSolver.Application;
using EquationsSolver.Application.Presenters;
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
        if (!TryParseArgs(args, out var consoleOptions))
            return;
        if (!TryValidateArgs(consoleOptions, out var options))
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
            .AddTransient<ISolvingResultsPresenter, DefaultSolvingResultsPresenter>()
            //.AddTransient<ISolvingResultsPresenter, ConsoleSolvingResultsPresenter>()
            .AddTransient<App>()
            .BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<App>();
        app.Start();
    }

    private static bool TryParseArgs(string[] args, out ConsoleSolverOptions options)
    {
        var parser = new Parser(with =>
        {
            with.CaseInsensitiveEnumValues = true;
            with.HelpWriter = Console.Error;
        });

        var parserResult = parser.ParseArguments<ConsoleSolverOptions>(args);
        
        options = parserResult.Value;

        return parserResult.Tag == ParserResultType.Parsed;
    }

    private static bool TryValidateArgs(ConsoleSolverOptions consoleOptions, out SolverOptions options)
    {
        options = new SolverOptions();

        if (consoleOptions.EquationsFileName is not null && !File.Exists(consoleOptions.EquationsFileName))
        {
            Console.WriteLine($"Не найден файл с уравнениями. Путь '{consoleOptions.EquationsFileName}'.");
            return false;
        }

        if (consoleOptions.EquationsFileName is null && consoleOptions.ThreadsNumber is not null)
        {
            Console.WriteLine("Параллельное вычисление возможно только при чтении из файла (отсутствует параметр -f <path> или --file <path>).");
            return false;
        }

        options = new SolverOptions
        {
            EquationsFileName = consoleOptions?.EquationsFileName,
            ThreadsNumber = consoleOptions?.ThreadsNumber,
        };
        return true;
    }
}