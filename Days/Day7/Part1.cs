namespace AdventOfCode2023.Days.Day7;

internal class Part1 : DayPart
{
    //public override bool HasPrecedence => true;
    //public override string InputFile => "Example.txt";
    //public override bool ShouldRejectWhiteSpaceLines => false;

    public override void Run(List<string> input)
    {
        // List of hands
        // Order hands by strength of hand

        // Hand consists of 5 cards

        var hands = input.Select(HandBid.FromLine).ToList();
        hands.Sort();
        //hands.ForEach(Console.WriteLine);

        int winnings = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            winnings += hands[i].Bid * (i + 1);
        }

        Console.WriteLine(winnings);
    }

    private enum HandType : int
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    private record HandBid(string Hand, int Bid) : IComparable<HandBid>
    {
        public static HandBid FromLine(string line)
        {
            var split = line.Split(' ', StringSplitOptions.TrimEntries);
            string hand = split[0];
            int bid = int.Parse(split[1]);
            return new HandBid(hand, bid);
        }

        public int CompareTo(HandBid? other)
        {
            HandBid? x = this;
            HandBid? y = other;

            int xHandType = (int)x!.GetHandType();
            int yHandType = (int)y!.GetHandType();

            if (xHandType == yHandType)
            {
                for (int i = 0; i < 5; i++)
                {
                    char xCard = x!.Hand[i];
                    char yCard = y!.Hand[i];

                    int xRelStrength = GetRelativeStrength(xCard);
                    int yRelStrength = GetRelativeStrength(yCard);

                    if (xRelStrength == yRelStrength)
                    {
                        continue;
                    }

                    if (xRelStrength < yRelStrength)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }

                throw new Exception();
            }

            if (xHandType < yHandType)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private HandType GetHandType()
        {
            List<Occurances> occurances = [];
            for (int i = 0; i < 5; i++)
            {
                char card = Hand[i];

                bool isNew = true;
                for (int j = 0; j < occurances.Count; j++)
                {
                    if (occurances[j].Card == card)
                    {
                        isNew = false;
                        occurances[j] = new(card, occurances[j].Count + 1);
                        break;
                    }
                }

                if (isNew)
                {
                    occurances.Add(new(card, 1));
                }
            }

            occurances = [.. occurances.OrderByDescending(o => o.Count)];

            switch (occurances.Count)
            {
                case 1:
                    return HandType.FiveOfAKind;
                case 2:
                    if (occurances[0].Count == 4 && occurances[1].Count == 1)
                    {
                        return HandType.FourOfAKind;
                    }

                    if (occurances[0].Count == 3 && occurances[1].Count == 2)
                    {
                        return HandType.FullHouse;
                    }

                    throw new Exception();
                case 3:
                    if (occurances[0].Count == 3)
                    {
                        return HandType.ThreeOfAKind;
                    }

                    if (occurances[0].Count == 2 && occurances[1].Count == 2)
                    {
                        return HandType.TwoPair;
                    }

                    throw new Exception();
                case 4:
                    if (occurances[0].Count == 2)
                    {
                        return HandType.OnePair;
                    }

                    throw new Exception();
                case 5:
                    return HandType.HighCard;
                default:
                    throw new Exception();
            }
        }

        private record struct Occurances(char Card, int Count);

        public override string ToString()
        {
            return $"{Hand} {Bid}";
        }
    }

    public static int GetRelativeStrength(char card)
    {
        switch (card)
        {
            case 'A':
                return 14;
            case 'K':
                return 13;
            case 'Q':
                return 12;
            case 'J':
                return 11;
            case 'T':
                return 10;
            default:
                int digit = int.Parse([card]);

                if (digit == 1)
                {
                    throw new Exception("Card of \'1\' is not expected.");
                }

                return digit;
        }
    }
}
