namespace AdventOfCode2022;

internal class Day4 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        var resultSecondPart = 0;
        var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();
        foreach (var line in nonEmptyLines)
        {
            (var pair1, var pair2) = GetPairs(line!);
            var increment = pair1.IsSubsetOf(pair2) || pair2.IsSubsetOf(pair1) ? 1 : 0;
            result += increment;
            increment = pair1.Intersect(pair2).Any() ? 1 : 0;
            resultSecondPart += increment;
        }

        Console.WriteLine($"Day 4 result part 1: {result}");
        Console.WriteLine($"Day 4 result part 2: {resultSecondPart}");

        static (HashSet<int> Pair1, HashSet<int> Pair2) GetPairs(string line)
        {
            var pairs = InputReader.ProcessStringLine(line, ',');
            var pair1StartEnd = InputReader.ProcessStringLine(pairs[0], '-');
            var pair2StartEnd = InputReader.ProcessStringLine(pairs[1], '-');
            var pair1Start = Convert.ToInt32(pair1StartEnd[0]);
            var pair1End = Convert.ToInt32(pair1StartEnd[1]);
            var pair2Start = Convert.ToInt32(pair2StartEnd[0]);
            var pair2End = Convert.ToInt32(pair2StartEnd[1]);

            var pair1 = new HashSet<int>(Enumerable.Range(pair1Start, pair1End - pair1Start + 1));
            var pair2 = new HashSet<int>(Enumerable.Range(pair2Start, pair2End - pair2Start + 1));

            return (pair1, pair2);
        }
    }
}
