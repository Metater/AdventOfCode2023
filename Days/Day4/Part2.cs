namespace AdventOfCode2023.Days.Day4;

internal class Part2 : DayPart
{
    public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";

    public override void Run(List<string> input)
    {
        int cardIndex = 1;
        List<Card> cards = input.Select<string, Card>(l =>
        {
            string[] numbers = l.Split(':', StringSplitOptions.TrimEntries)[1].Split('|', StringSplitOptions.TrimEntries);

            HashSet<int> winning = numbers[0].Split(' ', StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse).ToHashSet();

            HashSet<int> held = numbers[1].Split(' ', StringSplitOptions.TrimEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse).ToHashSet();

            var matches = held.Where(n => winning.Contains(n)).Count();

            return new(cardIndex++, matches);
        }).ToList();

        cards.Reverse();

        // index, count
        Dictionary<int, int> cache = [];

        int count = 0;
        foreach (var card in cards)
        {
            int recursiveContribution = 1;
            for (int i = 1; i <= card.Matches; i++)
            {
                int index = card.Index + i;
                recursiveContribution += cache[index];
            }

            cache[card.Index] = recursiveContribution;

            count += recursiveContribution;
        }

        Console.WriteLine(count);
    }

    record Card(int Index, int Matches);
}
