using System.Reflection;
using AdventOfCode2023;
using AdventOfCode2023.Days;

#pragma warning disable CS0162 // Unreachable code detected

Console.WriteLine("Hello, World!");

const bool OnlyRunLastPart = false;

string path = $@"{Utils.GetParentDirectoryRecursive(Directory.GetCurrentDirectory(), 3)}\Days";
List<RunnablePart> parts = [];

var assembly = Assembly.GetCallingAssembly();
foreach (var type in assembly.GetTypes())
{
    if (type.BaseType == typeof(DayPart))
    {
        string[] split = type.ToString().Split('.');
        string day = split[2];
        string part = split[3];

        var instance = Activator.CreateInstance(type)!;
        string inputFile = (string)type.GetProperty("InputFile")!.GetValue(instance)!;
        string inputPath = $@"{path}\{day}\{inputFile}";
        parts.Add(new(inputPath, (DayPart)instance, day, part));
    }
}

parts = [.. parts.OrderBy(p => p.Day + p.Part)];

if (OnlyRunLastPart)
{
    var part = parts.LastOrDefault();
    if (part is not null)
    {
        Console.WriteLine($"Running {part.Day} {part.Part}...");

        var lines = File.ReadAllLines(part.InputPath);
        part.DayPart.Run([.. lines]);
    }
}
else
{
    foreach (var part in parts)
    {
        Console.WriteLine($"Running {part.Day} {part.Part}...");

        var lines = File.ReadAllLines(part.InputPath);
        part.DayPart.Run([.. lines]);
    }
}

record RunnablePart(string InputPath, DayPart DayPart, string Day, string Part);
