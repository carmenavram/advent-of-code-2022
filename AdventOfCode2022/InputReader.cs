using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal class InputReader
{
    public static IList<string?> ReadLines(string path)
    {
        var lines = new List<string?>();
        using var file = new StreamReader(path);
        string? line;

        do
        {
            line = file.ReadLine();
            lines.Add(line);
        }
        while (line is not null);

        return lines;
    }

    public static string[] ProcessStringLine(string line, char separator = ' ')
    {
        return line.Split(separator);
    }

    public static string[] ProcessStringLineString(string line, string separator = " ") => line.Split(separator);

    public static int[] GetNumbersFromLine(string line) => Regex.Split(line, @"[^-\d]").Where(n => !string.IsNullOrEmpty(n)).Select(n => Convert.ToInt32(n)).ToArray();
}
