using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public static GameStorage Instance { get; private set; }

    [Header("Data for start new game")]
    [SerializeField] GameData _initialGameData;
    public GameData InitialGameData => _initialGameData;

    [Header("Current game data")]
    [SerializeField] Locations _location;
    public Locations location { get => _location; set => _location = value; }

    [Range(0, 4)]
    [SerializeField] int _level;

    [Range(5, 10)]
    [SerializeField] int maxLevel = 5;
    public int level { get => _level; set => _level = value % maxLevel; }

    public bool isFirstLevel => _level == 0;
    public bool isLastLevel => _level == maxLevel - 1;

    public PlayerData PlayerData { get; set; }

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
