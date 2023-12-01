﻿namespace AdventOfCode2023.Days;

internal abstract class DayPart
{
    public virtual string InputFile => "Input.txt";

    public abstract void Run(List<string> input);
}