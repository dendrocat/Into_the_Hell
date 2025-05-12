using System;
using UnityEngine;

/// <summary>
/// Абстрактный менеджер уровней, отвечающий за управление текущей локацией и уровнем.
/// Использует паттерн Singleton для глобального доступа.
/// </summary>
public abstract class AbstractLevelManager : MonoBehaviour
{
    /// <summary>
    /// Экземпляр менеджера уровней (Singleton).
    /// </summary>
    public static AbstractLevelManager Instance { get; private set; } = null;


    [Header("Хранилище данных")]

    /// <summary>
    /// Ссылка на хранилище данных уровней.
    /// </summary>
    [Tooltip("Хранилище данных уровней")]
    [SerializeField] protected LevelStorage _levelStorage;


    [Header("Текущее местоположение")]

    /// <summary>
    /// Текущая локация.
    /// </summary>
    [Tooltip("Текущая локация")]
    [SerializeField] protected Location _location;

    /// <summary>
    /// Возвращает <see langword="true"/>, если текущая локация - финальная.
    /// </summary>
    public bool isFinalLocation => _location == Location.Final;


    /// <summary>
    /// Текущий уровень внутри локации.
    /// </summary>
    [Tooltip("Текущий уровень внутри локации")]
    [SerializeField, Range(0, 10)] int _currentLevel = 0;

    /// <summary>
    /// Максимальное количество уровней в локации.
    /// </summary>
    [Tooltip("Максимальное количество уровней в локации")]
    [SerializeField, Range(5, 10)] int maxLevel = 5;

    /// <summary>
    /// Текущий уровень с корректировкой по модулю <see cref="maxLevel"/>.
    /// </summary>
    int CurrentLevel { get => _currentLevel; set => _currentLevel = value % maxLevel; }

    /// <summary>
    /// Возвращает <see langword="true"/>, если текущий уровень - первый.
    /// </summary>
    public bool isFirstLevel => _currentLevel == 0;
    
    /// <summary>
    /// Возвращает <see langword="true"/>, если текущий уровень - последний.
    /// </summary>
    public bool isLastLevel => _currentLevel == maxLevel - 1;

    /// <summary>
    /// Валидация значений полей в редакторе.
    /// </summary>
    private void OnValidate() {
        if (_currentLevel >= maxLevel)
            _currentLevel = maxLevel - 1;
        if (_currentLevel < 0) _currentLevel = 0;
    }

    /// <summary>
    /// Инициализация Singleton и вызов инициализации уровня.
    /// </summary>
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        InitLevel();
    }

    /// <summary>
    /// Метод инициализации уровня. Может быть переопределён в наследниках.
    /// </summary>
    protected virtual void InitLevel() { }

    /// <summary>
    /// Абстрактный метод генерации уровня. Обязателен для реализации в наследниках.
    /// </summary>
    public abstract void Generate();

    /// <summary>
    /// Получает контейнер врагов для текущей локации.
    /// </summary>
    /// <returns><see cref="EnemyContainer">Контейнер врагов</see>.</returns>
    public EnemyContainer GetEnemies()
    {
        return _levelStorage.GetEnemyContainer(_location);
    }

    /// <summary>
    /// Переходит к следующему уровню.
    /// Если достигнут последний уровень, переключается на следующую локацию.
    /// </summary>
    public void NextLevel()
    {
        CurrentLevel++;
        if (isFirstLevel)
            _location = (Location)(((int)_location + 1) % Enum.GetValues(typeof(Location)).Length);
    }

    /// <summary>
    /// Устанавливает данные текущего уровня и локации.
    /// </summary>
    /// <param name="location">Локация.</param>
    /// <param name="level">Уровень внутри локации.</param>
    public void SetLevelData(Location location, int level)
    {
        _location = location;
        CurrentLevel = level;
    }

    /// <summary>
    /// Получает текущие данные уровня.
    /// </summary>
    /// <returns>Кортеж с локацией и уровнем (<see cref="Location"/>, <see cref="int"/>).</returns>
    public (Location, int) GetLevelData()
    {
        return (_location, CurrentLevel);
    }
}
