using System.IO;
using System.Collections.Generic;
using UnityEngine;
public class GameRepository
{
    private static readonly string _savePath = Path.Combine(
        Application.persistentDataPath, "save.json"
    );

    public static bool HasSave()
    {
        return File.Exists(_savePath);
    }

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath, json);
    }

    public static GameData Load()
    {
        if (!HasSave())
        {
            var data = new GameData();
            data.playerData.armorLevel = 1;
            data.playerData.potionLevel = 1;
            data.playerData.weapon = WeaponType.Sword;
            return data;
        }

        string json = File.ReadAllText(_savePath);
        return JsonUtility.FromJson<GameData>(json);
    }

}
