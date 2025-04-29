using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _nextBase;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void LoadStartLevel(bool isBase)
    {
        InputManager.Instance.PushInputMap(InputMap.Gameplay);
        _nextBase = !isBase;
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (isBase) SceneManager.LoadScene("BaseScene");
        else SceneManager.LoadScene("TutorialScene");
    }

    public void NewGame(bool tutorial)
    {
        Debug.Log("NewGame");
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();
        SaveLoadManager.Load();
        if (!tutorial) LoadStartLevel(isBase: true);
        else LoadStartLevel(isBase: false);
    }

    public void LoadGame()
    {
        SaveLoadManager.Load();
        LoadStartLevel(isBase: true);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameStorage.Instance.InstallPlayerData();
        if (scene.name.Contains("Level"))
            StartCoroutine(WaitInstance(() => LevelManager.Instance));
        else
            StartCoroutine(WaitInstance(() => StaticLevelManager.Instance));
    }

    IEnumerator WaitInstance(Func<ILevelManager> getInstance)
    {
        yield return new WaitUntil(() => getInstance() != null);
        getInstance().Generate(GameStorage.Instance.location);
    }

    public void NextLevel()
    {
        GameStorage.Instance.CollectPlayerData();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameStorage.Instance.level += Convert.ToInt32(_nextBase);
        if (GameStorage.Instance.isFirstLevel && _nextBase)
        {
            var location = GameStorage.Instance.location;
            location = (Locations)(((int)location + 1) % Enum.GetValues(typeof(Locations)).Length);
            GameStorage.Instance.location = location;
            if (location == Locations.Final)
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
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();
        Destroy(DontDestroyManager.Instance);
    }

    void OnDestroy()
    {
        Instance = null;
        SceneManager.LoadScene(0);
    }
}
