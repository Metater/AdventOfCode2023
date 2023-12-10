namespace AdventOfCode2023.Days.Day10;

internal class Part1 : DayPart
{
    public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        char GetTile(Position position)
        {
            return input[position.Y][position.X];
        }

        Func<Position, char> getTile = GetTile;

        int width = input[0].Length;
        int height = input.Count;

        int startingIndex = -1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Position position = new(x, y);

                if (getTile(position) == 'S')
                {
                    if (startingIndex == -1)
                    {
                        startingIndex = position.GetIndex(width, height);
                    }
                    else
                    {
                        throw new Exception("Multiple starting positions were found.");
                    }
                }
            }
        }

        Position startingPosition = Position.FromIndex(startingIndex, width, height);
        List<Pipe> startingPipeVariations = GetStartingPositionPipeVariations().ToList();


    }

    private static bool CanAcceptConnection(Pipe connection, )

    private static IEnumerable<Position> GetPipeSurroundingPositions(Position position, Pipe pipe, int width, int height)
    {
        if (pipe.HasFlag(Pipe.None))
        {
            yield break;
        }

        int connectionCount = 0;

        if (pipe.HasFlag(Pipe.North))
        {
            connectionCount++;
            Position offset = position.Offset(0, -1);
            offset.BoundsCheck(width, height);
            yield return offset;
        }

        if (pipe.HasFlag(Pipe.East))
        {
            connectionCount++;
            Position offset = position.Offset(1, 0);
            offset.BoundsCheck(width, height);
            yield return offset;
        }

        if (pipe.HasFlag(Pipe.South))
        {
            connectionCount++;
            Position offset = position.Offset(0, 1);
            offset.BoundsCheck(width, height);
            yield return offset;
        }

        if (pipe.HasFlag(Pipe.West))
        {
            connectionCount++;
            Position offset = position.Offset(-1, 0);
            offset.BoundsCheck(width, height);
            yield return offset;
        }

        if (connectionCount != 2)
        {
            throw new Exception();
        }
    }

    private static IEnumerable<Pipe> GetStartingPositionPipeVariations()
    {
        yield return Pipe.North & Pipe.South;
        yield return Pipe.East & Pipe.West;
        yield return Pipe.North & Pipe.East;
        yield return Pipe.North & Pipe.West;
        yield return Pipe.South & Pipe.West;
        yield return Pipe.South & Pipe.East;
    }

    private static Pipe CharToPipe(char c)
    {
        return c switch
        {
            '|' => Pipe.North & Pipe.South,
            '-' => Pipe.East & Pipe.West,
            'L' => Pipe.North & Pipe.East,
            'J' => Pipe.North & Pipe.West,
            '7' => Pipe.South & Pipe.West,
            'F' => Pipe.South & Pipe.East,
            '.' => throw new Exception(),
            'S' => throw new Exception(),
            _ => throw new Exception(),
        };
    }

    [Flags]
    private enum Pipe
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8,
    }

    // Steps should be for each path taken
    private class Steps(int width, int height)
    {
        private readonly int width = width;
        private readonly int height = height;
        // tile index, step count
        private readonly Dictionary<int, int> steps = [];

        public bool TryRecord(Position position, int stepCount)
        {
            int index = position.GetIndex(width, height);
            if (steps.TryGetValue(index, out _))
            {
                return false;
            }

            steps[index] = stepCount;
            return true;
        }

        public int GetHighestStepCount()
        {
            return steps.Values.Max();
        }
    }

    private record struct Position(int X, int Y)
    {
        // Top left corner is (0, 0)

        public readonly void BoundsCheck(int width, int height)
        {
            if (X < 0 || X >= width)
            {
                throw new Exception();
            }

            if (Y < 0 || Y >= height)
            {
                throw new Exception();
            }
        }

        public readonly int GetIndex(int width, int height)
        {
            BoundsCheck(width, height);

            return (width * Y) + X;
        }

        public static Position FromIndex(int index, int width, int height)
        {
            if (index < 0 || index >= (width * height))
            {
                throw new Exception();
            }

            int x = index % width;
            int y = index / width;

            return new Position(x, y);
        }

        public readonly Position Offset(int dx, int dy)
        {
            return new Position(X + dx, Y + dy);
        }
    }
}
