using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Менеджер хранения данных игры.
/// Реализует паттерн Singleton.
/// Хранит начальные и текущие игровые данные, обеспечивает их валидацию и синхронизацию с игровыми объектами.
/// </summary>
public class GameStorage : MonoBehaviour
{
    /// <summary>
    /// Статический экземпляр менеджера (Singleton).
    /// </summary>
    public static GameStorage Instance { get; private set; }

    [Header("Данные для начала новой игры")]

    /// <summary>
    /// <see cref="GameData">Данные</see>, используемые при старте новой игры.
    /// </summary>
    [Tooltip("Данные, используемые при старте новой игры")]
    [SerializeField] GameData _initialGameData;

    /// <summary>
    /// Публичное свойство для доступа к начальному игровому состоянию.
    /// </summary>
    public GameData InitialGameData => _initialGameData;


    [Header("Текущие игровые данные")]

    /// <summary>
    /// Текущие игровые <see cref="GameData">данные</see>, которые изменяются в процессе игры.
    /// </summary>
    [Tooltip("Текущие игровые данные")]
    [SerializeField] GameData _currentGameData;

    /// <summary>
    /// Публичное свойство для доступа и изменения текущих игровых данных.
    /// </summary>
    public GameData CurrentGameData { get => _currentGameData; set => _currentGameData = value; }

    /// <summary>
    /// Валидация игровых данных для корректного состояния.
    /// Исправляет некорректные значения, например, локацию и уровни оружия.
    /// </summary>
    /// <param name="data">Игровые данные для валидации.</param>
    void ValidateGameData(ref GameData data) {
        if (data.location == Location.Final)
            data.location = Location.HellCastle;

        if (data.weaponLevels == null) data.weaponLevels = new List<byte>{1, 1, 1};
        for (int i = 0; i < data.weaponLevels.Count; ++i)
            if (data.weaponLevels[i] <= 0) data.weaponLevels[i] = 1;
    }

    /// <summary>
    /// Метод вызывается в редакторе при изменении данных в инспекторе.
    /// Производит валидацию игровых данных.
    /// </summary>
    void OnValidate()
    {
        ValidateGameData(ref _initialGameData);
        ValidateGameData(ref _currentGameData);
    }

    /// <summary>
    /// Инициализация Singleton.
    /// </summary>
    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Сбор текущих игровых данных из игровых объектов.
    /// </summary>
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

    /// <summary>
    /// Установка игровых данных в игровые объекты.
    /// </summary>
    public void InstallGameData()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null) return;
        AbstractLevelManager.Instance.SetLevelData(
            CurrentGameData.location,
            CurrentGameData.level
        );
        WeaponStorage.Instance.SetWeaponLevels(_currentGameData.weaponLevels);

        player.inventory.SetPlayerData(_currentGameData.playerData);
        player.inventory.SetPlayerWeapon(
            WeaponStorage.Instance.GetWeapon(_currentGameData.playerData.weapon)
        );
    }

    /// <summary>
    /// Очистка Singleton при уничтожении объекта.
    /// </summary>
    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
