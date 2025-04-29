using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Locations _location;
    public Locations Location => _location;

    [Range(0, 4)]
    [SerializeField] int _level;
    [SerializeField] int maxLevel;

    bool _nextBase;

    void LoadBaseScene()
    {
        InputManager.Instance.PushInputMap(InputMap.Gameplay);
        _nextBase = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("BaseScene");
    }

    public void NewGame()
    {
        Debug.Log("NewGame");
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();
        LoadGame();
    }

    public void StartTutorial()
    {
        Debug.Log("Tutorial");
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadGame()
    {
        SaveLoadManager.Load();
        LoadBaseScene();
    }

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
        _location = Locations.FrozenCaves;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //LoadData();
        //Debug.Log(scene.name);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerStorage.Instance.InstallPlayerData();
        if (!scene.name.Contains("Base"))
            LevelManager.Instance.Generate(
                                    _location,
                                    _level == maxLevel - 1
                                );
        else
            BaseRoomManager.Instance.SwapTiles(_location);
    }

    public void NextLevel()
    {
        PlayerStorage.Instance.CollectPlayerData();
        SceneManager.sceneLoaded += OnSceneLoaded;
        _level = (_level + Convert.ToInt32(_nextBase)) % maxLevel;
        if (_level == 0 && _nextBase)
        {
            _location = (Locations)(((int)_location + 1) % Enum.GetValues(typeof(Locations)).Length);
            if (_location == Locations.Final)
            {
                SceneManager.LoadScene("FinalScene");
                return;
            }
        }
        if (_nextBase)
        {
            SaveLoadManager.Save();
            SceneManager.LoadScene("BaseScene");
        }
        else
            SceneManager.LoadScene("LevelScene");
        _nextBase = !_nextBase;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(1);
        InputManager.Instance.PopInputMap();
    }

    public static void ReloadGame()
    {
        SceneManager.LoadScene(0);
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public (Locations, int) GetPlayerProgress()
    {
        return (_location, _level);
    }

    public void SetPlayerProgress(Locations location, int level)
    {
        _location = location;
        _level = level;
    }
}
