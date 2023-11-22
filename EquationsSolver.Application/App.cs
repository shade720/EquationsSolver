using System.Collections.Concurrent;
using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.Application;

public class App
{
    private readonly SolverOptions _options;
    private readonly IEquationReaderFactory _equationReaderFactory;
    private readonly IEquationSolver _equationSolver;
    private readonly ISolvingResultsPresenter _solvingResultsPresenter;

    public App(
        SolverOptions options,
        IEquationReaderFactory equationReaderFactory,
        IEquationSolver equationSolver,
        ISolvingResultsPresenter solvingResultsPresenter)
    {
        _options = options;
        _equationReaderFactory = equationReaderFactory;
        _equationSolver = equationSolver;
        _solvingResultsPresenter = solvingResultsPresenter;
    }

    public void Start()
    {
        var equationsReader = _equationReaderFactory.CreateEquationsReader(_options.EquationsFileName);

        if (_options.EquationsFileName is null || _options.ThreadsNumber is null)
            SequentialMode(equationsReader, _solvingResultsPresenter);
        else
            ParallelMode(equationsReader, _solvingResultsPresenter);
    }

    private void SequentialMode(IEquationsReader equationsReader, ISolvingResultsPresenter solvingResultsPresenter)
    {
        foreach (var equation in equationsReader.Read())
        {
            var solvingResult = _equationSolver.Solve(equation);
            solvingResultsPresenter.ShowResults(solvingResult);
        }
    }

    private void ParallelMode(IEquationsReader equationsReader, ISolvingResultsPresenter solvingResultsPresenter)
    {
        var results = new ConcurrentBag<EquationSolvingResult>();
        Parallel.ForEach(
            equationsReader.Read().ToArray(),
            new ParallelOptions { MaxDegreeOfParallelism = _options.ThreadsNumber!.Value },
            equation => results.Add(_equationSolver.Solve(equation))
        );
        foreach (var solvingResult in results)
            solvingResultsPresenter.ShowResults(solvingResult);
    }
}