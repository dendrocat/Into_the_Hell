using UnityEngine;

public class PlayerStorage : MonoBehaviour
{
    public static PlayerStorage Instance { get; private set; }

    [SerializeField] PlayerData _playerData;

    public PlayerData PlayerData { get => _playerData; set => _playerData = value; }

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
    }

    public void CollectPlayerData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        PlayerData = player.inventory.GetPlayerData();
    }

    public void InstallPlayerData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        player.inventory.SetPlayerData(PlayerData);
        player.inventory.SetPlayerWeapon(
                    WeaponStorage.Instance.GetWeapon(PlayerData.weapon),
                    WeaponStorage.Instance.GetWeaponLevels()[(int)PlayerData.weapon]
                );
    }


    void OnDestroy()
    {
        Instance = null;
    }
}
