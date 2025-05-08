using System.Collections.Generic;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public static GameStorage Instance { get; private set; }

    [Header("Data for start new game")]
    [SerializeField] GameData _initialGameData;
    public GameData InitialGameData => _initialGameData;

    [Header("Current game data")]
    [SerializeField] GameData _currentGameData;
    public GameData GameData { get => _currentGameData; set => _currentGameData = value; }

    
    void ValidateGameData(ref GameData data) {
        if (data.location == Location.Final)
            data.location = Location.HellCastle;

        if (data.weaponLevels == null) data.weaponLevels = new List<byte>{1, 1, 1};
        for (int i = 0; i < data.weaponLevels.Count; ++i)
            if (data.weaponLevels[i] <= 0) data.weaponLevels[i] = 1;
    }

    void OnValidate()
    {
        ValidateGameData(ref _initialGameData);
        ValidateGameData(ref _currentGameData);
    }


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
        _currentGameData.location = location;
        _currentGameData.level = level;
        _currentGameData.weaponLevels = WeaponStorage.Instance.GetWeaponLevels();

        _currentGameData.playerData = player.inventory.GetPlayerData();
    }

    public void InstallGameData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        AbstractLevelManager.Instance.SetLevelData(
            GameData.location,
            GameData.level
        );
        WeaponStorage.Instance.SetWeaponLevels(_currentGameData.weaponLevels);

        player.inventory.SetPlayerData(_currentGameData.playerData);
        player.inventory.SetPlayerWeapon(
            WeaponStorage.Instance.GetWeapon(_currentGameData.playerData.weapon)
        );
    }


    void OnDestroy()
    {
        Instance = null;
    }
}
