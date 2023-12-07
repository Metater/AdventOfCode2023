namespace AdventOfCode2023.Days.Day4;

internal class Part1 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";

    public override void Run(List<string> input)
    {
        int i = 0;
        List<Card> cards = input.Select<string, Card>(l =>
        {
            int index = i++;

            string[] numbers = l.Split(':', StringSplitOptions.TrimEntries)[1].Split('|', StringSplitOptions.TrimEntries);

            HashSet<int> winning = numbers[0].Split(' ', StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse).ToHashSet();

            HashSet<int> held = numbers[1].Split(' ', StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse).ToHashSet();

            return new(index, winning, held);
        }).ToList();

        int points = cards.Sum(c =>
        {
            var matching = c.Held.Where(n => c.Winning.Contains(n)).ToList();

            if (matching.Count > 0)
            {
                return 1 << matching.Count - 1;
            }

            return 0;
        });

        Console.WriteLine(points);

        //foreach (var card in cards)
        //{
        //    Console.WriteLine(card.Index);

        //    Console.WriteLine("Winning:");
        //    foreach (var w in card.Winning)
        //    {
        //        Console.WriteLine($"\t{w}");
        //    }

        //    Console.WriteLine("Held:");
        //    foreach (var h in card.Held)
        //    {
        //        Console.WriteLine($"\t{h}");
        //    }
        //}
    }

    record Card(int Index, HashSet<int> Winning, HashSet<int> Held);
}
