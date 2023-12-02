using System.Diagnostics;
using System.Reflection;
using AdventOfCode2023;
using AdventOfCode2023.Days;

Console.WriteLine("Hello, World!");
Console.WriteLine("--------------------------------");

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

if (OnlyRunLastPart && parts.Count > 0)
{
    parts = [parts.Last()];

    Console.WriteLine("Only running last part!");
    Console.WriteLine("--------------------------------");
}
else if (parts.Any(p => p.DayPart.HasPrecedence))
{
    parts = parts.Where(p => p.DayPart.HasPrecedence).ToList();

    Console.WriteLine("Giving precedence to specific day part(s):");
    foreach (var part in parts)
    {
        Console.WriteLine($"{part.Day} {part.Part}");
    }
    Console.WriteLine("--------------------------------");
}

foreach (var part in parts)
{
    Console.WriteLine($"{part.Day} {part.Part} output below:");

    var lines = File.ReadAllLines(part.InputPath)
        .Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

    Console.WriteLine($"Loaded {lines.Count} lines. Running...");

    Stopwatch stopwatch = Stopwatch.StartNew();
    part.DayPart.Run(lines);
    stopwatch.Stop();

    Console.WriteLine($"{part.Day} {part.Part} took {stopwatch.Elapsed.TotalMilliseconds}ms to run.");
    Console.WriteLine("--------------------------------");
}

record RunnablePart(string InputPath, DayPart DayPart, string Day, string Part);
