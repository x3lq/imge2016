using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour
{
    private static string path = "/song.gd";

    public static void run(bool save, bool load)
    {
        if (save)
        {
            SaveLoad.Save();
        }
        if (load)
        {
            SaveLoad.Load();
        }
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);
        bf.Serialize(file, Song.current);
        file.Close();

        Debug.Log("Saved...");
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            Song.current = (Song)bf.Deserialize(file);
            file.Close();

            EventHandler.current.Song = Song.current;

            Debug.Log("Loaded...");
        }
    }
}
