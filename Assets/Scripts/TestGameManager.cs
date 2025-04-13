using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        LoadGame();
    }
    public void LoadGame()
    {
        Debug.Log("LoadGame");
        var data = GameRepository.Load();
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInventory>().SetPlayerData(data.playerData);
        TestWeaponStorage.Instance.SetWeaponLevels(data.weaponLevels);
    }

    public void SaveGame()
    {
        var data = new GameData();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent(out PlayerInventory inventory))
        {
            data.playerData = inventory.GetPlayerData();
        }
        data.weaponLevels = TestWeaponStorage.Instance.GetWeaponLevels();
        GameRepository.Save(data);
    }
}
