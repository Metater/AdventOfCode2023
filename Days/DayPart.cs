namespace AdventOfCode2023.Days;

internal abstract class DayPart
{
    public virtual bool HasPrecedence => false;
    public virtual string InputFile => "Input.txt";
    public virtual bool ShouldRejectWhiteSpaceLines => true;

    public abstract void Run(List<string> input);
}