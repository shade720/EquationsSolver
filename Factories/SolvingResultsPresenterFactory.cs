using EquationsSolver.Abstractions;
using EquationsSolver.Models.Presenters;

namespace EquationsSolver.Factories;

public class SolvingResultsPresenterFactory : ISolvingResultsPresenterFactory
{
    public ISolvingResultsPresenter CreateEquationPresenter() 
        => new ConsoleSolvingResultsPresenter();
}