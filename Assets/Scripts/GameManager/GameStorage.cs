using System.Collections.Generic;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public static GameStorage Instance { get; private set; }

    [Header("Data for start new game")]
    [SerializeField] GameData _initialGameData;
    public GameData InitialGameData => _initialGameData;

    [Header("Current game data")]
    [SerializeField] GameData gameData;
    public GameData GameData { get => gameData; set => gameData = value; }

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
    }

    public void CollectGameData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        (var location, var level) = AbstractLevelManager.Instance.GetLevelData();
        gameData.location = (int)location;
        gameData.level = level;
        gameData.weaponLevels = WeaponStorage.Instance.GetWeaponLevels();

        gameData.playerData = player.inventory.GetPlayerData();
    }

    public void InstallGameData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        AbstractLevelManager.Instance.SetLevelData(
            (Location)GameData.location,
            GameData.level
        );
        WeaponStorage.Instance.SetWeaponLevels(gameData.weaponLevels);

        player.inventory.SetPlayerData(gameData.playerData);
        player.inventory.SetPlayerWeapon(
            WeaponStorage.Instance.GetWeapon(gameData.playerData.weapon)
        );
    }


    void OnDestroy()
    {
        Instance = null;
    }
}
