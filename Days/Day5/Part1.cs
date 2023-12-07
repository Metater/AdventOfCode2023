namespace AdventOfCode2023.Days.Day5;

internal class Part1 : DayPart
{
    public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        // Input is the almanac
        // lists seeds to be planted
        // lists types of soil to use with each seed
        List<long> seedsToBePlanted = input[0].Split(' ', StringSplitOptions.TrimEntries)[1..].Select(long.Parse).ToList();

        input = input[2..].Where(l => string.IsNullOrWhiteSpace(l) || !char.IsAsciiLetterLower(l[0])).ToList();

        List<List<Mapping>> maps = [];
        {
            List<Mapping> map = [];
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    maps.Add(map);
                    map = [];
                    continue;
                }

                map.Add(Mapping.FromLine(line));
            }

            maps.Add(map);
        }

        //int i = 0;
        //foreach (var item in maps)
        //{
        //    Console.WriteLine(i++);
        //    foreach (var it in item)
        //    {
        //        Console.WriteLine($"\t{it}");
        //    }
        //}

        //Map test = new(50, 98, 2);
        //Console.WriteLine(test.AppliesToSource(97));
        //Console.WriteLine(test.AppliesToSource(98));
        //Console.WriteLine(test.AppliesToSource(99));
        //Console.WriteLine(test.AppliesToSource(100));
        //Console.WriteLine(test.GetDestinationOfApplicableSource(98));
        //Console.WriteLine(test.GetDestinationOfApplicableSource(99));

        List<long> sources = [.. seedsToBePlanted];
        List<long> destinations = [];
        foreach (var map in maps)
        {
            foreach (var mapping in map)
            {
                sources.RemoveAll(source =>
                {
                    if (mapping.AppliesToSource(source))
                    {
                        long destination = mapping.GetDestinationOfApplicableSource(source);
                        destinations.Add(destination);
                        return true;
                    }

                    return false;
                });
            }

            destinations.AddRange(sources);
            sources.Clear();
            sources.AddRange(destinations);
            destinations.Clear();
        }

        sources.Sort();

        Console.WriteLine(sources[0]);
    }

    record Mapping(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
    {
        public bool AppliesToSource(long source)
        {
            return source >= SourceRangeStart && source < (SourceRangeStart + RangeLength);
        }

        public long GetDestinationOfApplicableSource(long source)
        {
            long relativeIndex = source - SourceRangeStart;
            return DestinationRangeStart + relativeIndex;
        }

        public static Mapping FromLine(string line)
        {
            var split = line.Split(' ', StringSplitOptions.TrimEntries);
            long d = long.Parse(split[0]);
            long s = long.Parse(split[1]);
            long r = long.Parse(split[2]);
            return new Mapping(d, s, r);
        }

        public override string ToString()
        {
            return $"DestRangeStart:{DestinationRangeStart}|SrcRangeStart:{SourceRangeStart}|RangeLen:{RangeLength}";
        }
    }
}
