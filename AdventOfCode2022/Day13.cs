namespace AdventOfCode2022;

internal partial class Day13 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        var resultSecondPart = 0;
        var rightOrder = 0;
        var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();

        for (int i = 0; i < nonEmptyLines.Count; i += 2)
        {
            var pairIndex = i / 2 + 1;
            var packet1 = nonEmptyLines[i];
            var packet2 = nonEmptyLines[i + 1];
            if (Packet.InRightOrder(packet1!, packet2!, true))
            {
                rightOrder += pairIndex;
                Console.WriteLine($"Pair {pairIndex} is in the right order");
            }
            else
            {
                Console.WriteLine($"Pair {pairIndex} is NOT in the right order");
            }
            Console.WriteLine();
            Console.WriteLine("*************************");
        }

        result = rightOrder;
        Console.WriteLine($"Day 13 result part 1: {result}");

        List<string> dividers = new() { "[[2]]", "[[6]]" };
        nonEmptyLines.AddRange(dividers);

        var packets = nonEmptyLines.Select(l => new Packet(l!)).ToList();
        packets.Sort();
        resultSecondPart = 1;

        for (int i = 0; i < packets.Count; i++)
        {
            if (dividers.Contains(packets[i].Value))
            {
                var dividerIndex = i + 1;
                resultSecondPart *= dividerIndex;
            }
            // Console.WriteLine(packets[i]);
        }

        Console.WriteLine($"Day 13 result part 2: {resultSecondPart}");
    }
}
