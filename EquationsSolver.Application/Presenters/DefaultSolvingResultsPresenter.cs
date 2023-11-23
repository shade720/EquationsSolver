using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Presenters;

public sealed class DefaultSolvingResultsPresenter : ISolvingResultsPresenter
{
    private readonly ILogger _logger;

    public DefaultSolvingResultsPresenter(ILogger logger)
    {
        _logger = logger;
    }

    public void ShowResults(EquationSolvingResult result)
    {
        if (!result.IsSolvedSuccessful)
        {
            _logger.LogError("Не удалось решить уравнение '{0}'. Возможно уравнение имеет неверную степень.", result.OriginalEquation);
            return;
        }
        var roots = result.Roots.ToArray();
        switch (roots.Length)
        {
            case 0:
                _logger.LogInformation("Уравнение {0} не имеет действтельных корней",
                    result.OriginalEquation);
                break;
            case 1:
                _logger.LogInformation("Уравнение {0} имеет 1 действительный корень - {1}",
                    result.OriginalEquation, roots[0]);
                break;
            case > 1:
                _logger.LogInformation("Уравнение {0}. Действительных корней - {1}. Корни: {2}", 
                    result.OriginalEquation, roots.Length, string.Join(", ", roots));
                break;
        }
    }
}