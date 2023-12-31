﻿namespace AdventOfCode2023.Days.Day8;

internal class Part1 : DayPart
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

        int i = 0;
        string currentNode = "AAA";
        while (true)
        {
            if (cache.TryGetValue(currentNode, out var pair))
            {
                bool shouldGoRight = instructionIterator.GetNext();
                if (shouldGoRight)
                {
                    currentNode = pair.Right;
                }
                else
                {
                    currentNode = pair.Left;
                }

                i++;

                if (currentNode == "ZZZ")
                {
                    break;
                }
            }
            else
            {
                throw new Exception();
            }
        }

        Console.WriteLine(i);
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
}
