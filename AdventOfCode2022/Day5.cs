using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal class Day5 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = string.Empty;
        var resultSecondPart = 0;

        var emptyLineIndex = inputLines.IndexOf(string.Empty);
        var numberOfStacks = InputReader.ProcessStringLineString(inputLines[emptyLineIndex - 1]!, "   ").Length;
        var stacks = new List<Stack<string>>();

        for (int j = 0; j < numberOfStacks; j++)
        {
            stacks.Add(new Stack<string>());
        }

        for (int i = emptyLineIndex - 2; i >= 0; i--)
        {
            var line = inputLines[i];
            for (int j = 0; j < numberOfStacks; j++)
            {
                var value = line.Substring(4 * j, 3);
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    stacks[j].Push(RemoveBrackets(value));
                }
            }
        }

        var instructions = GetInstructions(inputLines, emptyLineIndex);

        foreach (var instruction in instructions)
        {
            for (int i = 0; i < instruction.NumberOfItems; i++)
            {
                var itemToMove = stacks[instruction.Origin].Pop();
                stacks[instruction.Destination].Push(itemToMove);
            }
        }

        foreach (var stack in stacks)
        {
            result += stack.Pop();
        }

        Console.WriteLine($"Day 5 result part 1: {result}");
        Console.WriteLine($"Day 5 result part 2: {resultSecondPart}");

        static string RemoveBrackets(string value) => value.Replace("[", string.Empty).Replace("]", string.Empty);

        static List<(int NumberOfItems, int Origin, int Destination)> GetInstructions(IList<string?> inputLines, int emptyLineIndex)
        {
            List<(int NumberOfItems, int Origin, int Destination)> instructions = new();
            for (int i = emptyLineIndex + 1; i < inputLines.Count; i++)
            {
                var line = inputLines[i];
                if (!string.IsNullOrEmpty(line))
                {
                    var numbers = Regex.Split(line, @"\D+").Where(n => !string.IsNullOrEmpty(n)).Select(n => Convert.ToInt32(n)).ToArray();
                    instructions.Add((numbers[0], numbers[1] - 1, numbers[2] - 1));
                }
            }

            return instructions;
        }
    }
}
