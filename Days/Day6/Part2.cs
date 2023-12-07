namespace AdventOfCode2023.Days.Day6;

internal class Part2 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        long time = Parse(input[0]);
        long recordDistance = Parse(input[1]);
        //Console.WriteLine(time + " " + recordDistance);

        Console.WriteLine((long)(GetB(time, recordDistance) - GetA(time, recordDistance)) + 1);
    }

    private static double GetA(double r, double d)
    {
        return -(-r + GetSqrtPart(r, d)) / 2;
    }

    private static double GetB(double r, double d)
    {
        return -(-r - GetSqrtPart(r, d)) / 2;
    }

    private static double GetSqrtPart(double r, double d)
    {
        return Math.Sqrt(Math.Pow(r, 2) - (4 * d));
    }

    //private static long GetDistance(long time, long buttonReleaseMs)
    //{
    //    long travellingTime = time - buttonReleaseMs;
    //    long speed = buttonReleaseMs;
    //    return travellingTime * speed;
    //}

    private static long Parse(string line)
    {
        line = line.Split(':', StringSplitOptions.TrimEntries)[1];
        return long.Parse(line.Replace(" ", ""));
    }
}
