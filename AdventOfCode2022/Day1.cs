namespace AdventOfCode2022;

internal class Day1 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        var sortedTotalCaloriesSet = new SortedSet<int>();
        var totalCaloriesPerElf = 0;
        foreach (var line in inputLines)
        {
            if (int.TryParse(line, out int calorie))
            {
                totalCaloriesPerElf += calorie;
            }
            else
            {
                sortedTotalCaloriesSet.Add(totalCaloriesPerElf);
                totalCaloriesPerElf = 0;
            }
        }
        result = sortedTotalCaloriesSet.Max;
        Console.WriteLine($"Day 1 result part 1: {result}");
        var sortedList = sortedTotalCaloriesSet.ToList();
        result += sortedList[^2] + sortedList[^3];
        Console.WriteLine($"Day 1 result part 2: {result}");
    }
}
