
namespace AdventOfCode2023.Days.Day2;

internal class Part1 : DayPart
{
    //public override string InputFile => "Example.txt";

    public override void Run(List<string> input)
    {
        // RGB Cubes
        // Secret # of cubes in the bag
        // Find information about the # of cubes
        // Elf will grab random cubes, show them to you, put them back, a few times

        List<Game> games = input.Select<string, Game>(l =>
        {
            int id = -1;
            List<Round> rounds = [];

            string[] splitByColons = l.Split(':', StringSplitOptions.TrimEntries);
            string gameAndId = splitByColons[0];

            {
                string[] splitBySpaces = gameAndId.Split(' ', StringSplitOptions.TrimEntries);
                id = int.Parse(splitBySpaces[1]);
            }

            {
                string info = splitByColons[1];
                string[] splitBySemicolons = info.Split(';', StringSplitOptions.TrimEntries);
                foreach (var round in splitBySemicolons)
                {
                    int red, green, blue;
                    red = green = blue = 0;

                    string[] splitByCommas = round.Split(',', StringSplitOptions.TrimEntries);
                    foreach (var countAndColor in splitByCommas)
                    {
                        string[] splitBySpaces = countAndColor.Split(' ', StringSplitOptions.TrimEntries);
                        int count = int.Parse(splitBySpaces[0]);
                        string color = splitBySpaces[1];
                        switch (color)
                        {
                            case "red":
                                red += count;
                                break;
                            case "green":
                                green += count;
                                break;
                            case "blue":
                                blue += count;
                                break;
                        }
                    }

                    rounds.Add(new(red, green, blue));
                }
            }

            return new(id, rounds);
        }).ToList();

        //foreach (var game in games)
        //{
        //    Console.WriteLine($"Id: {game.Id}");
        //    foreach (var round in game.Rounds)
        //    {
        //        Console.WriteLine($"Round: R: {round.Red} G: {round.Green} B: {round.Blue}");
        //    }
        //}

        var possibleGames = games.Where(g =>
        {
            foreach (var round in g.Rounds)
            {
                if (round.Red > 12)
                {
                    return false;
                }

                if (round.Green > 13)
                {
                    return false;
                }

                if (round.Blue > 14)
                {
                    return false;
                }
            }

            return true;
        }).ToList();

        int idSum = possibleGames.Sum(g => g.Id);

        Console.WriteLine(idSum);
    }

    private record Game(int Id, List<Round> Rounds);
    private record Round(int Red, int Green, int Blue);
}
