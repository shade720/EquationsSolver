using EquationsSolver.Models;

namespace EquationsSolver.Abstractions;

public interface ISolvingResultsPresenter
{
    public void ShowResults(EquationSolvingResult result);
}