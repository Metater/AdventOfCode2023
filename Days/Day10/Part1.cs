namespace AdventOfCode2023.Days.Day10;

internal class Part1 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        char GetTile(Position position)
        {
            return input[position.Y][position.X];
        }

        int width = input[0].Length;
        int height = input.Count;

        int startingIndex = -1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Position position = new(x, y);

                if (GetTile(position) == 'S')
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
            if (startingPipeVariation == (Pipe.East | Pipe.West))
            {
                _ = 42;
            }

            var surrounding = GetPipeSurroundingPositions(startingPosition, startingPipeVariation, width, height)
                .ToList();
            bool isCorrectPipeVariation = surrounding.All(p =>
            {
                return CanAcceptConnection(p.fromDirection, p.toPosition, GetTile, out _);
            });

            if (isCorrectPipeVariation && surrounding.Count == 2)
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

        Steps steps = new(width, height);
        int stepCount = 0;
        while (true)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                (Pipe fromDirection, Position toPosition) = paths[i];
                var pos = toPosition;
                if (!CanAcceptConnection(fromDirection, toPosition, GetTile, out var toDirection))
                {
                    throw new Exception();
                }

                (fromDirection, toPosition) = Trace(toDirection, toPosition, GetTile, width, height);
                paths[i] = (fromDirection, toPosition);
                //Console.WriteLine(GetTile(toPosition));

                if (!steps.TryRecord(pos, stepCount))
                {
                    goto Done;
                }
            }

            stepCount++;
        }

    Done:;

        Console.WriteLine(stepCount + 1);

        //var pathSteps = new Steps[paths.Count];
        //int steps = 0;
        //while (true)
        //{
        //    for (int i = 0; i < paths.Count; i++)
        //    {
        //        (Pipe fromDirection, Position toPosition) = paths[i];
        //        if (!CanAcceptConnection(fromDirection, toPosition, GetTile, out var toDirection))
        //        {
        //            throw new Exception();
        //        }

        //        Pipe toPipe = CharToPipe(GetTile(toPosition));
        //        var next = GetPipeSurroundingPositions(toPosition, toPipe, width, height)
        //            .Where(p => p.fromDirection == toDirection)
        //            .ToList();

        //        if (next.Count != 1)
        //        {
        //            throw new Exception();
        //        }

        //        paths[i] = next.First();

        //        _ = 42;
        //    }

        //    steps++;
        //}
    }

    private static (Pipe fromDirection, Position toPosition) Trace(Pipe toDirection, Position toPosition, Func<Position, char> getTile, int width, int height)
    {
        char c = getTile(toPosition);
        Pipe pipe = CharToPipe(c);

        Pipe nextDirection = pipe ^ toDirection;
        switch (nextDirection)
        {
            case Pipe.North:
                Position offset = toPosition.Offset(0, -1);
                offset.XYBoundsCheck(width, height);
                return (Pipe.North, offset);
            case Pipe.East:
                offset = toPosition.Offset(1, 0);
                offset.XYBoundsCheck(width, height);
                return (Pipe.East, offset);
            case Pipe.South:
                offset = toPosition.Offset(0, 1);
                offset.XYBoundsCheck(width, height);
                return (Pipe.South, offset);
            case Pipe.West:
                offset = toPosition.Offset(-1, 0);
                offset.XYBoundsCheck(width, height);
                return (Pipe.West, offset);
            default:
                throw new Exception();
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
            if (offset.IsWithinBounds(width, height))
                yield return (Pipe.North, offset);
        }

        if (pipe.HasFlag(Pipe.East))
        {
            connectionCount++;
            Position offset = position.Offset(1, 0);
            if (offset.IsWithinBounds(width, height))
                yield return (Pipe.East, offset);
        }

        if (pipe.HasFlag(Pipe.South))
        {
            connectionCount++;
            Position offset = position.Offset(0, 1);
            if (offset.IsWithinBounds(width, height))
                yield return (Pipe.South, offset);
        }

        if (pipe.HasFlag(Pipe.West))
        {
            connectionCount++;
            Position offset = position.Offset(-1, 0);
            if (offset.IsWithinBounds(width, height))
                yield return (Pipe.West, offset);
        }

        //if (connectionCount != 2)
        //{
        //    throw new Exception();
        //}
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

    private class Steps(int width, int height)
    {
        // tile index, step count
        private readonly Dictionary<int, int> steps = [];

        public bool TryRecord(Position position, int stepCount)
        {
            int index = position.GetIndex(width, height);
            if (steps.ContainsKey(index))
            {
                return false;
            }

            steps.Add(index, stepCount);
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

        public readonly bool IsWithinBounds(int width, int height)
        {
            if (X < 0 || X >= width)
            {
                return false;
            }

            if (Y < 0 || Y >= height)
            {
                return false;
            }

            return true;
        }

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
