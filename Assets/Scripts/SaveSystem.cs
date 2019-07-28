using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(int level)
    {
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/level.txt";
        var stream = new FileStream(path, FileMode.Create);
        var progress = new Progress(level);
        formatter.Serialize(stream, progress);
        stream.Close();
    }

    public static Progress Load()
    {
        var path = Application.persistentDataPath + "/level.txt";
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);
            var progress = formatter.Deserialize(stream) as Progress;
            stream.Close();
            return progress;
        }
        else
        {
            return new Progress(1);
        }
    }
}