namespace AdventOfCode2023.Days.Day8;

internal class Part2 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        // Puzzle input is a bunch of documents, maps
        // They help with desert navigation

        // One document:
        // List of left/right instructions
        // The rest:
        // Describe a network of labeled nodes

        string leftRightInstructions = input[0].Trim();
        InstructionIterator instructionIterator = new(leftRightInstructions);
        List<Node> network = input[1..].Select(Node.FromLine).ToList();

        //network.ForEach(n => Console.WriteLine(n));

        // self, leftRightPair
        Dictionary<string, LeftRightPair> cache = [];
        foreach (Node node in network)
        {
            cache.Add(node.Self, node.Pair);
        }

        List<string> currentNodes = network.Select(n => n.Self).Where(s => s.EndsWith('A')).ToList();

        //currentNodes.ForEach(Console.WriteLine);

        long[] paths = new long[currentNodes.Count];

        long i = 0;
        while (true)
        {
            bool shouldGoRight = instructionIterator.GetNext();

            for (int j = 0; j < currentNodes.Count; j++)
            {
                string currentNode = currentNodes[j];
                if (cache.TryGetValue(currentNode, out var pair))
                {
                    if (shouldGoRight)
                    {
                        currentNode = pair.Right;
                    }
                    else
                    {
                        currentNode = pair.Left;
                    }
                }
                else
                {
                    throw new Exception();
                }

                currentNodes[j] = currentNode;

                if (currentNode[2] == 'Z')
                {
                    if (paths[j] == 0)
                    {
                        paths[j] = i;
                    }
                }
            }

            i++;

            if (paths.All(p => p != 0))
            {
                break;
            }
        }

        for (int j = 0; j < paths.Length; j++)
        {
            paths[j] = paths[j] + 1;
        }

        Console.WriteLine(LcmArray(paths));

        //for (int j = 0; j < paths.Length; j++)
        //{
        //    Console.WriteLine(j);
        //    Console.WriteLine(paths[j][0]);
        //    for (int k = 1; k < paths[j].Count; k++)
        //    {
        //        Console.WriteLine($"\t{paths[j][k] - paths[j][k - 1]}");
        //    }
        //}

        // Attempted:
        // 1584096016678838644
    }

    private class InstructionIterator(string instructions)
    {
        private readonly string instructions = instructions;
        private int pointer = 0;

        // False is left
        // True is right
        public bool GetNext()
        {
            if (pointer >= instructions.Length)
            {
                pointer = 0;
            }

            char next = instructions[pointer];
            pointer++;

            return next switch
            {
                'L' => false,
                'R' => true,
                _ => throw new Exception(),
            };
        }
    }

    private record struct LeftRightPair(string Left, string Right)
    {
        public static LeftRightPair FromPair(string pair)
        {
            var leftRight = pair.Substring(1, 8).Split(',', StringSplitOptions.TrimEntries);
            string left = leftRight[0];
            string right = leftRight[1];
            return new(left, right);
        }

        public readonly override string ToString()
        {
            return $"Left:{Left}|Right:{Right}";
        }
    }

    private record struct Node(string Self, LeftRightPair Pair)
    {
        public static Node FromLine(string line)
        {
            var splitByEquals = line.Split('=', StringSplitOptions.TrimEntries);

            string self = splitByEquals[0];
            var pair = LeftRightPair.FromPair(splitByEquals[1]);

            return new(self, pair);
        }

        public readonly override string ToString()
        {
            return $"Self:{Self}|{Pair}";
        }
    }

    //private static long Gcf(long a, long b)
    //{
    //    while (b != 0)
    //    {
    //        long temp = b;
    //        b = a % b;
    //        a = temp;
    //    }

    //    return a;
    //}

    //private static long Lcm(long a, long b)
    //{
    //    return (a / Gcf(a, b)) * b;
    //}

    private static long LcmArray(long[] numbers)
    {
        return numbers.Aggregate(Lcm);
    }

    private static long Lcm(long a, long b)
    {
        return Math.Abs(a * b) / Gcd(a, b);
    }

    private static long Gcd(long a, long b)
    {
        return b == 0 ? a : Gcd(b, a % b);
    }
}
