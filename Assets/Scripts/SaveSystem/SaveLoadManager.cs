using System;
using UnityEngine;

/// <summary>
/// Manages game state persistence between scenes and sessions
/// </summary>
/// <remarks>
/// Bridges between game objects and <see cref="SaveLoadRepository"/> for data serialization.
/// </remarks>
public class SaveLoadManager
{

    /// <summary>
    /// Restores game state from persistent storage
    /// </summary>
    public static void Load()
    {
        var data = SaveLoadRepository.Load();
        var player = GameObject.FindGameObjectWithTag("Player");
        TestWeaponStorage.Instance.SetWeaponLevels(data.weaponLevels);
        player.GetComponent<PlayerInventory>().SetPlayerData(data.playerData);
    }

    /// <summary>
    /// Captures current game state and persists it
    /// </summary>
    public static void Save()
    {
        var data = SaveLoadRepository.Load();
        data.weaponLevels = TestWeaponStorage.Instance.GetWeaponLevels();

        var player = GameObject.FindGameObjectWithTag("Player");
        var inventory = player.GetComponent<PlayerInventory>();
        data.playerData = inventory.GetPlayerData();
        Debug.Log(inventory.GetPlayerWeapon().GetType().Name);
        var type = Enum.Parse<WeaponType>(inventory.GetPlayerWeapon().GetType().Name);
        data.weaponLevels[(int)type] = inventory.GetPlayerWeapon().level;

        SaveLoadRepository.Save(data);
    }
}
