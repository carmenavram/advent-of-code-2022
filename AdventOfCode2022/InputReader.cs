﻿namespace AdventOfCode2022;

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

}
