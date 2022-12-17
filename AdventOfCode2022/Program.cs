using AdventOfCode2022;
using System.Reflection;

string[] commandLineArgs = Environment.GetCommandLineArgs();
var dayNumber = commandLineArgs.Length > 1 ? Convert.ToInt32(commandLineArgs[1]) : 1;
Console.WriteLine($"Solving day {dayNumber}");
Console.WriteLine();

var inputFolder = Path.Combine(Environment.CurrentDirectory, @"input");
var inputFile = $"day{dayNumber}_input.txt";
var inputLines = InputReader.ReadLines(Path.Combine(inputFolder, inputFile));

IDay selectedStrategy = Instantiate<IDay>($"Day{dayNumber}");
selectedStrategy.Solve(inputLines);

static T Instantiate<T>(string className)
{
    Type typeImplemented = typeof(T);
    Type selectedType = Assembly.GetExecutingAssembly() 
            .GetTypes()
            .First(t => typeImplemented.IsAssignableFrom(t) && t.Name == className);

    return (T)Activator.CreateInstance(selectedType);
}

