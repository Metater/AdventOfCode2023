namespace AdventOfCode2023.Days.Day2;

internal class Part2 : DayPart
{
    //public override string InputFile => "Example.txt";

    public override void Run(List<string> input)
    {
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

        var fewestCubesToMakeGamePossibleSum = games.Sum(g =>
        {
            int fewestRed, fewestGreen, fewestBlue;
            fewestRed = fewestGreen = fewestBlue = 0;

            foreach (var round in g.Rounds)
            {
                if (round.Red > fewestRed)
                {
                    fewestRed = round.Red;
                }

                if (round.Green > fewestGreen)
                {
                    fewestGreen = round.Green;
                }

                if (round.Blue > fewestBlue)
                {
                    fewestBlue = round.Blue;
                }
            }

            return fewestRed * fewestGreen * fewestBlue;
        });

        Console.WriteLine(fewestCubesToMakeGamePossibleSum);
    }

    private record Game(int Id, List<Round> Rounds);
    private record Round(int Red, int Green, int Blue);
}
