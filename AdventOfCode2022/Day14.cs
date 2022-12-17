namespace AdventOfCode2022;

internal class Day14 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        var resultSecondPart = 0;
        var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();

        Console.WriteLine($"Day 14 result part 1: {result}");
        Console.WriteLine($"Day 14 result part 2: {resultSecondPart}");
    }
}
