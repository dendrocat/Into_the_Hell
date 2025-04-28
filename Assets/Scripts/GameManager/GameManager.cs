using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Locations _location;
    public Locations Location => _location;

    [Range(0, 4)]
    [SerializeField] int level;
    [SerializeField] int maxLevel;

    bool _nextBase;

    [SerializeField] bool _hasSave;

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
        LoadBaseScene();
    }

    public void StartTutorial()
    {
        Debug.Log("Tutorial");
        SceneManager.LoadScene("Tutorial");
    }

    public bool LoadGame()
    {
        if (!_hasSave)
        {
            Debug.Log("No Saves");
            return false;
        }
        LoadBaseScene();
        return true;
    }

    void LoadData()
    {
        Debug.Log("Load Game Data");
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
                                    level == maxLevel - 1
                                );
        else
            BaseRoomManager.Instance.SwapTiles(_location);
    }

    public void NextLevel()
    {
        PlayerStorage.Instance.CollectPlayerData();
        SceneManager.sceneLoaded += OnSceneLoaded;
        level = (level + Convert.ToInt32(_nextBase)) % maxLevel;
        if (level == 0 && _nextBase)
        {
            _location = (Locations)(((int)_location + 1) % Enum.GetValues(typeof(Locations)).Length);
            if (_location == Locations.Final)
            {
                SceneManager.LoadScene("FinalScene");
                return;
            }
        }
        if (_nextBase)
            SceneManager.LoadScene("BaseScene");
        else
            SceneManager.LoadScene("LevelScene");
        _nextBase = !_nextBase;
        //_player.transform.position = Vector3.zero;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(1);
        InputManager.Instance.PopInputMap();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
