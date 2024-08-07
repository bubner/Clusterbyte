using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Save file data serialization and deserialization.
/// </summary>
public static class SaveFile
{
    private const string FILENAME = "save.json";

    [Serializable]
    public class Save
    {
        public int[] completedLevelTimesMillis;
        public int lives;

        public Save(int lives, int[] completedLevelTimesMillis)
        {
            this.lives = lives;
            this.completedLevelTimesMillis = completedLevelTimesMillis;
        }
    }

    private static Save GetDefaultSave()
    {
        return new Save(Clusterbyte.STARTING_LIVES, new int[Clusterbyte.LEVELS_TERRAIN.Length]);
    }

    public static Save Load()
    {
        if (!File.Exists(Path.Join(Application.dataPath, FILENAME)))
        {
            return GetDefaultSave();
        }
        string json = File.ReadAllText(Path.Join(Application.dataPath, FILENAME));
        Save save = JsonUtility.FromJson<Save>(json);
        Array.Resize(ref save.completedLevelTimesMillis, Clusterbyte.LEVELS_TERRAIN.Length);
        return save;
    }

    public static void Reset()
    {
        SaveData(GetDefaultSave());
    }

    public static void SaveData(Save save)
    {
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Path.Join(Application.dataPath, FILENAME), json);
    }
}