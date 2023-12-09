namespace AdventOfCode2023.Days.Day9;

internal class Part2 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        // Oasis And Sand Instability Sensor
        // OASIS produces values they are over time
        // Each line in the report contains the history of a single value

        var histories = input.Select(History.FromLine).ToList();

        long extrapolatedValueSum = 0;
        foreach (var history in histories)
        {
            Stack<History> stack = new([history]);
            while (!stack.Peek().IsAllZeroes())
            {
                History current = stack.Peek().GetDifferences();
                stack.Push(current);
            }

            extrapolatedValueSum += Extrapolate(stack);
        }

        Console.WriteLine(extrapolatedValueSum);
    }

    private static long Extrapolate(Stack<History> stack)
    {
        var lastValues = stack.Pop().Values;
        lastValues.Insert(0, 0);

        while (stack.TryPop(out var sequence))
        {
            var values = sequence.Values;
            values.Insert(0, values.First() - lastValues.First());
            lastValues = values;
        }

        return lastValues.First();
    }

    record History(List<long> Values)
    {
        public static History FromLine(string line)
        {
            var splitBySpaces = line.Split(' ', StringSplitOptions.TrimEntries);
            var values = splitBySpaces.Select(long.Parse).ToList();
            return new(values);
        }

        public History GetDifferences()
        {
            if (Values.Count < 2)
            {
                throw new Exception();
            }

            List<long> differences = [];
            for (var i = 1; i < Values.Count; i++)
            {
                long lastValue = Values[i - 1];
                long value = Values[i];
                differences.Add(value - lastValue);
            }

            return new(differences);
        }

        public bool IsAllZeroes()
        {
            return Values.All(v => v == 0);
        }

        public override string ToString()
        {
            return $"{string.Join(' ', Values)}";
        }
    }
}
