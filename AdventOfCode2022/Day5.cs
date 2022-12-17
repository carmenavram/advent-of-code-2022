using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal class Day5 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = string.Empty;
        var resultSecondPart = string.Empty;

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

        List<Stack<string>> secondPartStacks = CloneStacks(stacks);

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

        foreach (var instruction in instructions)
        {
            var itemsToMove = new Stack<string>();
            for (int i = 0; i < instruction.NumberOfItems; i++)
            {
                itemsToMove.Push(secondPartStacks[instruction.Origin].Pop());
            }
            while (itemsToMove.Count > 0)
            {
                var itemToMove = itemsToMove.Pop();
                secondPartStacks[instruction.Destination].Push(itemToMove);
            }
        }

        foreach (var stack in secondPartStacks)
        {
            resultSecondPart += stack.Pop();
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
                    var numbers = InputReader.GetNumbersFromLine(line);
                    instructions.Add((numbers[0], numbers[1] - 1, numbers[2] - 1));
                }
            }

            return instructions;
        }

        static List<Stack<string>> CloneStacks(List<Stack<string>> stacks)
        {
            var secondPartStacks = new List<Stack<string>>();
            foreach (var stack in stacks)
            {
                secondPartStacks.Add(new Stack<string>(stack.Reverse()));
            }

            return secondPartStacks;
        }
    }
}
