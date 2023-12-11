namespace AdventOfCode2023.Days.Day10;

internal class Part1 : DayPart
{
    public override bool HasPrecedence => true;
    public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        Func<Position, char> getTile = (Position position) =>
        {
            return input[position.Y][position.X];
        };

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
        Pipe correctStartingPipeVariant = Pipe.None;
        foreach (var startingPipeVariation in startingPipeVariations)
        {
            var surrounding = GetPipeSurroundingPositions(startingPosition, startingPipeVariation, width, height).ToList();
            bool isCorrectPipeVariation = surrounding.All(p =>
            {
                return CanAcceptConnection(p.fromDirection, p.toPosition, getTile, out _);
            });

            if (isCorrectPipeVariation)
            {
                correctStartingPipeVariant = startingPipeVariation;
                break;
            }
        }

        if (correctStartingPipeVariant == Pipe.None)
        {
            throw new Exception();
        }

        var paths = GetPipeSurroundingPositions(startingPosition, correctStartingPipeVariant, width, height).ToList();

        if (paths.Count != 2)
        {
            throw new Exception();
        }

        var pathSteps = new Steps[paths.Count];
        int steps = 0;
        while (true)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                (Pipe fromDirection, Position toPosition) = paths[i];
                if (!CanAcceptConnection(fromDirection, toPosition, getTile, out var toDirection))
                {
                    throw new Exception();
                }

                Pipe toPipe = CharToPipe(getTile(toPosition));
                var next = GetPipeSurroundingPositions(toPosition, toPipe, width, height)
                    .Where(p => p.fromDirection == toDirection)
                    .ToList();

                if (next.Count != 1)
                {
                    throw new Exception();
                }

                paths[i] = next.First();

                _ = 42;
            }

            steps++;
        }
    }

    private static bool CanAcceptConnection(Pipe fromDirection, Position toPosition, Func<Position, char> getTile, out Pipe toDirection)
    {
        char c = getTile(toPosition);
        Pipe toPipe = CharToPipe(c);

        switch (fromDirection)
        {
            case Pipe.North:
                toDirection = Pipe.South;
                return toPipe.HasFlag(Pipe.South);
            case Pipe.East:
                toDirection = Pipe.West;
                return toPipe.HasFlag(Pipe.West);
            case Pipe.South:
                toDirection = Pipe.North;
                return toPipe.HasFlag(Pipe.North);
            case Pipe.West:
                toDirection = Pipe.East;
                return toPipe.HasFlag(Pipe.East);
            default:
                throw new Exception();
        }
    }

    private static IEnumerable<(Pipe fromDirection, Position toPosition)> GetPipeSurroundingPositions(Position position, Pipe pipe, int width, int height)
    {
        if (pipe == Pipe.None)
        {
            throw new Exception();
        }

        int connectionCount = 0;

        if (pipe.HasFlag(Pipe.North))
        {
            connectionCount++;
            Position offset = position.Offset(0, -1);
            offset.XYBoundsCheck(width, height);
            yield return (Pipe.North, offset);
        }

        if (pipe.HasFlag(Pipe.East))
        {
            connectionCount++;
            Position offset = position.Offset(1, 0);
            offset.XYBoundsCheck(width, height);
            yield return (Pipe.East, offset);
        }

        if (pipe.HasFlag(Pipe.South))
        {
            connectionCount++;
            Position offset = position.Offset(0, 1);
            offset.XYBoundsCheck(width, height);
            yield return (Pipe.South, offset);
        }

        if (pipe.HasFlag(Pipe.West))
        {
            connectionCount++;
            Position offset = position.Offset(-1, 0);
            offset.XYBoundsCheck(width, height);
            yield return (Pipe.West, offset);
        }

        if (connectionCount != 2)
        {
            throw new Exception();
        }
    }

    private static IEnumerable<Pipe> GetStartingPositionPipeVariations()
    {
        yield return CharToPipe('|');
        yield return CharToPipe('-');
        yield return CharToPipe('L');
        yield return CharToPipe('J');
        yield return CharToPipe('7');
        yield return CharToPipe('F');
    }

    private static Pipe CharToPipe(char c)
    {
        return c switch
        {
            '|' => Pipe.North | Pipe.South,
            '-' => Pipe.East | Pipe.West,
            'L' => Pipe.North | Pipe.East,
            'J' => Pipe.North | Pipe.West,
            '7' => Pipe.South | Pipe.West,
            'F' => Pipe.South | Pipe.East,
            '.' => Pipe.None,
            'S' => Pipe.None,
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

        public readonly void XYBoundsCheck(int width, int height)
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
            XYBoundsCheck(width, height);

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
