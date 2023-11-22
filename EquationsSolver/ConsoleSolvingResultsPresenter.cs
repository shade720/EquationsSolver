using EquationsSolver.Domain.Abstractions;
using EquationsSolver.Domain.Models;

namespace EquationsSolver.ConsoleUI;

public class ConsoleSolvingResultsPresenter : ISolvingResultsPresenter
{
    public void ShowResults(EquationSolvingResult result)
    {
        if (!result.IsSolvedSuccessful)
        {
            Console.WriteLine($"Не удалось решить уравнение '{result.OriginalEquation}'. Возможно уравнение имеет неверную степень.");
            return;
        }
        var roots = result.Roots.ToArray();
        switch (roots.Length)
        {
            case 0:
                Console.WriteLine($"Уравнение {result.OriginalEquation} не имеет действтельных корней");
                break;
            case 1:
                Console.WriteLine($"Уравнение {result.OriginalEquation} имеет 1 действительный корень - {roots[0]}");
                break;
            case > 1:
                Console.WriteLine($"Уравнение {result.OriginalEquation}. Действительных корней - {roots.Length}. Корни: {string.Join(", ", roots)}");
                break;
        }
    }
}