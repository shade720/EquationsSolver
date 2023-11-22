using EquationsSolver.Domain.Models;

namespace EquationsSolver.Domain.Abstractions;

public interface ISolvingResultsPresenter
{
    public void ShowResults(EquationSolvingResult result);
}