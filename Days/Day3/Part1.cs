
using System.Text;

namespace AdventOfCode2023.Days.Day3;

internal class Part1 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";

    public override void Run(List<string> input)
    {
        // Add up part numbers to find missing
        List<char> grid = input.SelectMany(x => x).ToList();
        var schematic = new EngineSchematic(grid, input[0].Length, input.Count);

        char GetChar(int xx, int yy) => schematic!.Grid[xx + (yy * schematic.Height)];

        HashSet<int> numberRootIndicies = [];

        var symbols = schematic.GetSymbols().ToList();
        foreach ((var symbol, var coords) in symbols)
        {
            foreach ((var x, var y) in schematic.GetAdjacentNumbers(coords))
            {
                var rootX = x;

                while (rootX > 0)
                {
                    rootX--;

                    if (char.IsDigit(GetChar(rootX, y)))
                    {
                        continue;
                    }
                    else
                    {
                        rootX++;
                        break;
                    }
                }

                numberRootIndicies.Add(rootX + (y * schematic.Height));

                //Console.WriteLine(GetChar(rootX, y));
                //Console.WriteLine($"({x}, {y})");
                //Console.WriteLine();
            }
        }

        List<int> partNumbers = [];

        StringBuilder sb = new();
        foreach (var index in numberRootIndicies)
        {
            (var x, var y) = schematic.IndexToCoords(index);
            sb.Append(grid[index]);
            while (x < schematic.Width - 1)
            {
                x++;

                char c = GetChar(x, y);
                if (char.IsDigit(GetChar(x, y)))
                {
                    sb.Append(c);
                    continue;
                }
                else
                {
                    x--;
                    break;
                }
            }

            partNumbers.Add(int.Parse(sb.ToString()));
            sb.Clear();
        }

        Console.WriteLine(partNumbers.Sum());
    }

    record EngineSchematic(List<char> Grid, int Width, int Height)
    {
        public IEnumerable<(char c, (int x, int y))> GetSymbols()
        {
            for (int i = 0; i < Grid.Count; i++)
            {
                char c = Grid[i];

                if (char.IsDigit(c) || c == '.')
                {
                    continue;
                }

                var coords = IndexToCoords(i);
                i++;
                yield return (c, coords);
            }
        }

        public IEnumerable<(int x, int y)> GetAdjacentNumbers((int x, int y) coords)
        {
            (var x, var y) = coords;

            foreach ((var dx, var dy) in GetSearchOffsets())
            {
                var newX = x + dx;
                var newY = y + dy;
                if (CoordsWithinBounds((newX, newY)))
                {
                    int searchIndex = newX + (newY * Height);
                    char c = Grid[searchIndex];
                    if (char.IsDigit(c))
                    {
                        yield return (newX, newY);
                    }
                }
            }
        }

        public static IEnumerable<(int dx, int dy)> GetSearchOffsets()
        {
            yield return (1, 0);
            yield return (-1, 0);
            yield return (1, 1);
            yield return (1, -1);
            yield return (-1, 1);
            yield return (0, -1);
            yield return (-1, -1);
            yield return (0, 1);
        }

        public bool CoordsWithinBounds((int x, int y) coords)
        {
            (var x, var y) = coords;
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;
            return true;
        }

        public (int x, int y) IndexToCoords(int index)
        {
            return (index % Width, index / Height);
        }
    }
}
