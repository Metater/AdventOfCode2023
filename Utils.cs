namespace AdventOfCode2023;

internal static class Utils
{
    public static string GetParentDirectoryRecursive(string path, int i = 1)
    {
        string parent = Directory.GetParent(path)!.FullName;

        i--;
        if (i <= 0)
        {
            return parent;
        }

        return GetParentDirectoryRecursive(parent, i);
    }
}