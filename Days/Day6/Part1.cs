namespace AdventOfCode2023.Days.Day6;

internal class Part1 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        var times = input[0].Split(':', StringSplitOptions.TrimEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(int.Parse).ToList();
        var distances = input[1].Split(':', StringSplitOptions.TrimEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(int.Parse).ToList();

        List<Race> races = [];

        for (int i = 0; i < times.Count; i++)
        {
            races.Add(new Race(times[i], distances[i]));
        }

        //times.ForEach(Console.WriteLine);
        //distances.ForEach(Console.WriteLine);
        //races.ForEach(Console.WriteLine);

        List<int> waysToBeatRecordForEachRace = [];
        foreach (var race in races)
        {
            int currentRecordMillimeters = race.Dist;
            int waysToBeatRecord = 0;

            for (int i = 0; i < race.Time; i++)
            {
                int releaseButtonAtMilliseconds = i;

                int positionMillimeters = 0;
                int speedMillimetersPerMillisecond = 0;
                for (int j = 0; j < race.Time; j++)
                {
                    int millisecond = j;

                    if (millisecond >= releaseButtonAtMilliseconds)
                    {
                        positionMillimeters += speedMillimetersPerMillisecond;
                    }
                    else
                    {
                        speedMillimetersPerMillisecond++;
                    }
                }

                if (positionMillimeters > currentRecordMillimeters)
                {
                    waysToBeatRecord++;
                }
            }

            waysToBeatRecordForEachRace.Add(waysToBeatRecord);
        }

        int product = 1;
        foreach (var item in waysToBeatRecordForEachRace)
        {
            product *= item;
        }

        Console.WriteLine(product);
    }

    record Race(int Time, int Dist)
    {
        public override string ToString()
        {
            return $"Time:{Time}|Dist:{Dist}";
        }
    }
}
