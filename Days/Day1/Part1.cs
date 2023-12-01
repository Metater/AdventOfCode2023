
namespace AdventOfCode2023.Days.Day1;

internal class Part1 : DayPart
{
    public override void Run(List<string> input)
    {
        var sum = input
            .Where(l => !string.IsNullOrEmpty(l))
            .Sum(l =>
            {
                char firstDigit = l.First(char.IsDigit);
                char lastDigit = l.Last(char.IsDigit);

                return long.Parse(firstDigit + "" + lastDigit);
            });

        Console.WriteLine(sum);
    }
}
