using UnityEngine;
using System.IO;

public static class PathConverter
{
    public static string ToAssetPath(string path)
    {
        return Path.GetFullPath(path)
                   .Replace('\\', '/')
                   .Replace(Application.dataPath, "Assets");
    }

    public static string ToAbsolutePath(string path)
    {
        return Path.GetFullPath(path)
                   .Replace('\\', '/');
    }
}


