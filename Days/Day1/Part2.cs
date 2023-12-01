
namespace AdventOfCode2023.Days.Day1;

internal class Part2 : DayPart
{
    //public override string InputFile => "Example.txt";

    private static readonly Dictionary<string, char> Digits = new()
    {
        ["one"] = '1',
        ["two"] = '2',
        ["three"] = '3',
        ["four"] = '4',
        ["five"] = '5',
        ["six"] = '6',
        ["seven"] = '7',
        ["eight"] = '8',
        ["nine"] = '9'
    };

    public override void Run(List<string> input)
    {
        var sum = input
            .Where(l => !string.IsNullOrEmpty(l))
            .Sum(l =>
            {
                char first = ' ';
                for (int i = 0; i < l.Length; i++)
                {
                    if (char.IsDigit(l[i]))
                    {
                        first = l[i];
                        break;
                    }

                    foreach ((string spelling, char c) in Digits)
                    {
                        if (i + spelling.Length > l.Length)
                        {
                            continue;
                        }

                        if (l.Substring(i, spelling.Length) == spelling)
                        {
                            first = c;
                            break;
                        }
                    }

                    if (first != ' ')
                    {
                        break;
                    }
                }

                char last = ' ';
                for (int i = l.Length - 1; i >= 0; i--)
                {
                    if (char.IsDigit(l[i]))
                    {
                        last = l[i];
                        break;
                    }

                    foreach ((string spelling, char c) in Digits)
                    {
                        if (i + spelling.Length > l.Length)
                        {
                            continue;
                        }

                        if (l.Substring(i, spelling.Length) == spelling)
                        {
                            last = c;
                            break;
                        }
                    }

                    if (last != ' ')
                    {
                        break;
                    }
                }

                return long.Parse(first + "" + last);
            });

        Console.WriteLine(sum);
    }
}
