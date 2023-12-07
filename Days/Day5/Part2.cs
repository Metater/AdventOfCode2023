namespace AdventOfCode2023.Days.Day5;

internal class Part2 : DayPart
{
    //public override bool HasPrecedence => true;
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

        List<SourceRange> initialSourceRanges = [];
        int pairsCount = seedsToBePlanted.Count / 2;
        for (int i = 0; i < pairsCount; i++)
        {
            initialSourceRanges.Add(SourceRange.FromList(seedsToBePlanted));
        }

        //foreach (var item in sourceRanges)
        //{
        //    Console.WriteLine(item);
        //}

        //Mapping t = new(5, 3, 12);
        //List<SourceRange> inc = [];
        //List<SourceRange> exc = [];
        //t.GetSegmentsOfSourceRange(new(4, 11), inc, exc);
        //inc.ForEach(r => Console.WriteLine($"Inc:{r}"));
        //exc.ForEach(r => Console.WriteLine($"Exc:{r}"));

        List<SourceRange> sourceRanges = [.. initialSourceRanges];
        foreach (var map in maps)
        {
            List<SourceRange> successfullyMapped = [];
            foreach (var mapping in map)
            {
                List<SourceRange> addToNextIter = [];
                foreach (var sr in sourceRanges)
                {
                    List<SourceRange> inc = [];
                    List<SourceRange> exc = [];
                    mapping.GetSegmentsOfSourceRange(sr, inc, exc);
                    inc.RemoveAll(s => s.Length <= 0);
                    exc.RemoveAll(s => s.Length <= 0);
                    for (int i = 0; i < inc.Count; i++)
                    {
                        long relativeIndex = inc[i].Start - mapping.SourceRangeStart;
                        inc[i] = new(mapping.DestinationRangeStart + relativeIndex, inc[i].Length);
                    }

                    successfullyMapped.AddRange(inc);
                    addToNextIter.AddRange(exc);
                }

                sourceRanges.Clear();
                sourceRanges.AddRange(addToNextIter);
            }

            sourceRanges.AddRange(successfullyMapped);
        }


        var answer = sourceRanges.OrderBy(s => s.Start).First();

        Console.WriteLine(answer.Start);

        //List<long> sources = [.. seedsToBePlanted];
        //List<long> destinations = [];
        //foreach (var map in maps)
        //{
        //    foreach (var mapping in map)
        //    {
        //        sources.RemoveAll(source =>
        //        {
        //            if (mapping.AppliesToSource(source))
        //            {
        //                long destination = mapping.GetDestinationOfApplicableSource(source);
        //                destinations.Add(destination);
        //                return true;
        //            }

        //            return false;
        //        });
        //    }

        //    destinations.AddRange(sources);
        //    sources.Clear();
        //    sources.AddRange(destinations);
        //    destinations.Clear();
        //}

        //sources.Sort();

        //Console.WriteLine(sources[0]);
    }

    record struct SourceRange(long Start, long Length)
    {
        public static SourceRange FromList(List<long> pairs)
        {
            long s = pairs[0];
            pairs.RemoveAt(0);
            long l = pairs[0];
            pairs.RemoveAt(0);
            return new(s, l);
        }

        public readonly override string ToString()
        {
            return $"Start:{Start}|Length:{Length}";
        }
    }

    record Mapping(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
    {
        public void GetSegmentsOfSourceRange(SourceRange sourceRange, List<SourceRange> includedSegments, List<SourceRange> excludedSegments)
        {
            if (sourceRange.Length == 0)
            {
                throw new Exception("Source range length is zero");
            }

            if (RangeLength == 0)
            {
                throw new Exception("Range length is zero");
            }

            long narrowStartInclusive = sourceRange.Start;
            long narrowEndInclusive = sourceRange.Start + sourceRange.Length - 1;

            long broadStartInclusive = SourceRangeStart;
            long broadEndInclusive = SourceRangeStart + RangeLength - 1;

            bool overlap = narrowStartInclusive <= broadEndInclusive && narrowEndInclusive >= broadStartInclusive;

            if (!overlap)
            {
                excludedSegments.Add(sourceRange);
                return;
            }

            if (broadStartInclusive == narrowStartInclusive && broadEndInclusive == narrowEndInclusive)
            {
                includedSegments.Add(sourceRange);
                return;
            }

            if (broadStartInclusive == narrowStartInclusive)
            {
                long end = Math.Min(narrowEndInclusive, broadEndInclusive);
                SourceRange included = new(broadStartInclusive, end - broadStartInclusive + 1);
                includedSegments.Add(included);

                if (narrowEndInclusive > broadEndInclusive)
                {
                    long excludedLength = narrowEndInclusive - broadEndInclusive;
                    excludedSegments.Add(new(broadEndInclusive + 1, excludedLength));
                }

                return;
            }

            if (broadEndInclusive == narrowEndInclusive)
            {
                long start = Math.Max(narrowStartInclusive, broadStartInclusive);
                SourceRange included = new(start, broadEndInclusive - start + 1);
                includedSegments.Add(included);

                if (narrowStartInclusive < broadStartInclusive)
                {
                    long excludedLength = broadStartInclusive - narrowStartInclusive;
                    excludedSegments.Add(new(narrowStartInclusive, excludedLength));
                }

                return;
            }

            if (narrowStartInclusive < broadStartInclusive)
            {
                // Exc the first segment
                long excludeEndIndex = broadStartInclusive;
                excludedSegments.Add(new(narrowStartInclusive, excludeEndIndex - narrowStartInclusive));
                long includeStartIndex = broadStartInclusive;
                if (narrowEndInclusive < broadEndInclusive)
                {
                    long includeEndIndex = narrowEndInclusive;
                    includedSegments.Add(new(includeStartIndex, includeEndIndex - includeStartIndex + 1));
                    return;
                }
                else
                {
                    includedSegments.Add(new(includeStartIndex, broadEndInclusive - includeStartIndex + 1));
                    excludedSegments.Add(new(broadEndInclusive + 1, narrowEndInclusive - broadEndInclusive));
                    return;
                }
            }
            else
            {
                // Forget the first segment
                if (broadEndInclusive < narrowEndInclusive)
                {
                    includedSegments.Add(new(narrowStartInclusive, broadEndInclusive - narrowStartInclusive + 1));
                    excludedSegments.Add(new(broadEndInclusive + 1, narrowEndInclusive - broadEndInclusive));
                    return;
                }
                else
                {
                    includedSegments.Add(new(narrowStartInclusive, narrowEndInclusive - narrowStartInclusive + 1));
                    return;
                }
            }
        }

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
