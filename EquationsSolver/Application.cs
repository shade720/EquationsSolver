using System.Collections.Concurrent;
using EquationsSolver.Abstractions;
using EquationsSolver.Models;
using EquationsSolver.Options;

namespace EquationsSolver;

public class Application
{
    private readonly SolverOptions _options;
    private readonly IEquationReaderFactory _equationReaderFactory;
    private readonly IEquationSolver _equationSolver;
    private readonly ISolvingResultsPresenterFactory _solvingResultsPresenterFactory;

    public Application(
        SolverOptions options,
        IEquationReaderFactory equationReaderFactory,
        IEquationSolver equationSolver,
        ISolvingResultsPresenterFactory solvingResultsPresenterFactory)
    {
        _options = options;
        _equationReaderFactory = equationReaderFactory;
        _equationSolver = equationSolver;
        _solvingResultsPresenterFactory = solvingResultsPresenterFactory;
    }

    public void Start()
    {
        var equationsReader = _equationReaderFactory.CreateEquationsReader(_options.EquationsFileName);
        var solvingResultsPresenter = _solvingResultsPresenterFactory.CreatePresenter();

        if (_options.EquationsFileName is null || _options.ThreadsNumber is null)
            SequentialMode(equationsReader, solvingResultsPresenter);
        else
            ParallelMode(equationsReader, solvingResultsPresenter);
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