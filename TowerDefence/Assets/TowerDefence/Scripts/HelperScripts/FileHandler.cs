using System.IO;
using UnityEngine;

public static class FileHandler
{
    public static string Path(string filename)
    {
        return $"{Application.persistentDataPath}/{filename}";
    }

    public static void Reset(string filename)
    {
        string path = Path(filename);

        if (File.Exists(path)) 
            File.Delete(path);
    }

    public static bool HasFile(string filename)
    {
        return File.Exists(Path(filename));
    }
}
