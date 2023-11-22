using EquationsSolver.Abstractions;

namespace EquationsSolver.Models.Presenters;

public class ConsoleSolvingResultsPresenter : ISolvingResultsPresenter
{
    public void ShowResults(EquationSolvingResult result)
    {
        switch (result.Roots)
        {
            case Roots.None:
                Console.WriteLine("Решение уравнения: нет корней.");
                break;
            case Roots.One:
                Console.WriteLine($"Решение уравнения: имеет 1 корень - {result.Root1}.");
                break;
            case Roots.Two:
                Console.WriteLine($"Решение уравнения: имеет 2 кореня - х1={result.Root1}; х2={result.Root2}.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}