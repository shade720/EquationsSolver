using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquationsSolver.Application.Presenters;

public class DefaultSolvingResultsPresenter : ISolvingResultsPresenter
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
            _logger.LogError($"Не удалось решить уравнение '{result.OriginalEquation}'. Возможно уравнение имеет неверную степень.");
            return;
        }
        var roots = result.Roots.ToArray();
        switch (roots.Length)
        {
            case 0:
                _logger.LogInformation($"Уравнение {result.OriginalEquation} не имеет действтельных корней");
                break;
            case 1:
                _logger.LogInformation($"Уравнение {result.OriginalEquation} имеет 1 действительный корень - {roots[0]}");
                break;
            case > 1:
                _logger.LogInformation($"Уравнение {result.OriginalEquation}. Действительных корней - {roots.Length}. Корни: {string.Join(", ", roots)}");
                break;
        }
    }
}