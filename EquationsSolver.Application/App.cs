using System.Collections.Concurrent;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application;

public sealed class App
{
    private readonly SolverOptions _options;
    private readonly ILogger _logger;
    private readonly IEquationReaderFactory _equationReaderFactory;
    private readonly IEquationSolver _equationSolver;
    private readonly ISolvingResultsPresenter _solvingResultsPresenter;

    public App(
        SolverOptions options,
        ILogger logger, 
        IEquationReaderFactory equationReaderFactory,
        IEquationSolver equationSolver,
        ISolvingResultsPresenter solvingResultsPresenter)
    {
        _options = options;
        _logger = logger;
        _equationReaderFactory = equationReaderFactory;
        _equationSolver = equationSolver;
        _solvingResultsPresenter = solvingResultsPresenter;
    }

    public void Start()
    {
        var equationsReader = _equationReaderFactory.CreateEquationsReader(_options.EquationsFileName);

        if (_options.EquationsFileName is null || _options.ThreadsNumber is null)
            SolveSequential(equationsReader, _equationSolver, _solvingResultsPresenter);
        else
            SolveParallel(equationsReader, _equationSolver, _solvingResultsPresenter);
    }

    private void SolveSequential(
        IEquationsReader equationsReader, 
        IEquationSolver solver, 
        ISolvingResultsPresenter solvingResultsPresenter)
    {
        _logger.LogInformation("Используется последовательный режим\r\n");
        foreach (var equation in equationsReader.Read())
        {
            var solvingResult = solver.Solve(equation);
            solvingResultsPresenter.ShowResults(solvingResult);
        }
    }

    private void SolveParallel(
        IEquationsReader equationsReader, 
        IEquationSolver solver, 
        ISolvingResultsPresenter solvingResultsPresenter)
    {
        _logger.LogInformation("Используется параллельный режим\r\n");
        var results = new ConcurrentBag<EquationSolvingResult>();
        Parallel.ForEach(
            equationsReader.Read(),
            new ParallelOptions { MaxDegreeOfParallelism = _options.ThreadsNumber!.Value },
            equation => results.Add(solver.Solve(equation))
        );
        foreach (var solvingResult in results)
            solvingResultsPresenter.ShowResults(solvingResult);
    }
}