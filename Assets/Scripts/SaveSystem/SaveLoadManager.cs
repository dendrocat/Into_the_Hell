using UnityEngine;

public class SaveLoadManager
{
    public static void Load()
    {
        var data = SaveLoadRepository.Load();
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInventory>().SetPlayerData(data.playerData);
        TestWeaponStorage.Instance.SetWeaponLevels(data.weaponLevels);
    }
    public static void Save()
    {
        var data = new GameData();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventory inventory))
        {
            data.playerData = inventory.GetPlayerData();
        }
        data.weaponLevels = TestWeaponStorage.Instance.GetWeaponLevels();
        SaveLoadRepository.Save(data);
    }
}
