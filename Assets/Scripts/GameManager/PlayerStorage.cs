using UnityEngine;

public class PlayerStorage : MonoBehaviour
{
    public static PlayerStorage Instance { get; private set; }

    public PlayerData PlayerData { get; private set; }

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
    }


    void OnDestroy()
    {
        Instance = null;
    }
}
