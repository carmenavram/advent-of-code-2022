namespace AdventOfCode2022;

internal class Day3 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var alphabetPriorities = GetAlphabetPriorities();

        var result = 0;
        var resultSecondPart = 0;
        var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();
        foreach (var line in nonEmptyLines)
        {
            HashSet<char> firstCompartment = new HashSet<char>(line![..(line!.Length / 2)]);
            HashSet<char> secondCompartment = new HashSet<char>(line![^(line!.Length / 2)..]);
            var typeToRearrange = firstCompartment.Intersect(secondCompartment).Single();
            result += alphabetPriorities[typeToRearrange];
        }

        for (int i = 0; i <= nonEmptyLines.Count - 3; i += 3)
        {
            HashSet<char> firstRucksack = new HashSet<char>(nonEmptyLines[i]!);
            HashSet<char> secondRucksack = new HashSet<char>(nonEmptyLines[i + 1]!);
            HashSet<char> thirdRucksack = new HashSet<char>(nonEmptyLines[i + 2]!);

            var badge = firstRucksack.Intersect(secondRucksack).Intersect(thirdRucksack).Single();
            resultSecondPart += alphabetPriorities[badge];
        }

        Console.WriteLine($"Day 3 result part 1: {result}");
        Console.WriteLine($"Day 3 result part 2: {resultSecondPart}");

        static Dictionary<char, int> GetAlphabetPriorities()
        {
            var priorities = new Dictionary<char, int>();
            var lowercasePriority = 1;
            for (int i = 97; i <= 122; i++)
            {
                // a-z
                priorities[(char)i] = lowercasePriority++;
            }
            for (int i = 65; i <= 90; i++)
            {
                // A-Z
                priorities[(char)i] = lowercasePriority++;
            }

            return priorities;
        }
    }
}
