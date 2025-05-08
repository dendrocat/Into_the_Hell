using System;
using UnityEngine;

public abstract class AbstractLevelManager : MonoBehaviour
{
    [SerializeField] protected LevelStorage _levelStorage;

    public static AbstractLevelManager Instance { get; private set; } = null;

    [SerializeField] protected Location _location;
    public bool isFinalLocation => _location == Location.Final;

    [SerializeField]
    [Range(0, 10)] int _currentLevel = 0;

    [SerializeField]
    [Range(5, 10)] int maxLevel = 5;

    int CurrentLevel { get => _currentLevel; set => _currentLevel = value % maxLevel; }

    public bool isFirstLevel => _currentLevel == 0;
    public bool isLastLevel => _currentLevel == maxLevel - 1;

    private void OnValidate() {
        if (_currentLevel >= maxLevel)
            _currentLevel = maxLevel - 1;
        if (_currentLevel < 0) _currentLevel = 0;
    }


    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        InitLevel();
    }

    protected virtual void InitLevel() { }

    public abstract void Generate();

    public EnemyContainer GetEnemies()
    {
        return _levelStorage.GetEnemyContainer(_location);
    }

    public void NextLevel()
    {
        CurrentLevel++;
        if (isFirstLevel)
            _location = (Location)(((int)_location + 1) % Enum.GetValues(typeof(Location)).Length);
    }

    public void SetLevelData(Location location, int level)
    {
        _location = location;
        CurrentLevel = level;
    }

    public (Location, int) GetLevelData()
    {
        return (_location, CurrentLevel);
    }
}
