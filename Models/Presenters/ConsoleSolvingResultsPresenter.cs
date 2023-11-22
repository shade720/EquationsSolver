﻿using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Presenters;

public class ConsoleSolvingResultsPresenter : ISolvingResultsPresenter
{
    public void ShowResults(EquationSolvingResult result)
    {
        if (!result.IsSolvedSuccessful)
        {
            Console.WriteLine($"Не удалось решить уравнение '{result.OriginalEquation}'. Возможно уравнение имеет неверный порядок.");
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