using EquationsSolver.Abstractions;
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

        foreach (var equation in equationsReader.Read())
        {
            var result = _equationSolver.Solve(equation);
            if (result.IsSolvedSuccessful)
                solvingResultsPresenter.ShowResults(result);
            else 
                Console.WriteLine($"Не удалось решить уравнение '{equation}'. Возможно уравнение имеет неверный порядок.");
        }
    }
}