namespace EquationsSolver.Abstractions;

public interface ISolveResultsPresenterFactory
{
    public ISolvingResultsPresenter CreateEquationPresenter();
}