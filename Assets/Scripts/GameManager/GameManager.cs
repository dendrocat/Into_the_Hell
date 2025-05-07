using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _nextBase;

    bool _isTutor;

    void OnPlayerDied(Person player)
    {
        Debug.Log($"Person is {player}");
        if (!(player is Player)) return;
        ReloadGame();
    }


    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Person.Died.AddListener(OnPlayerDied);
    }

    void LoadStartLevel(bool isBase)
    {
        InputManager.Instance.PushInputMap(InputMap.Gameplay);
        _isTutor = _nextBase = !isBase;
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (isBase) SceneManager.LoadScene("BaseScene");
        else SceneManager.LoadScene("TutorialScene");
    }

    public void NewGame(bool tutorial)
    {
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
        GameStorage.Instance.InstallGameData();
        StartCoroutine(WaitInstance(() => AbstractLevelManager.Instance));
    }

    IEnumerator WaitInstance(Func<AbstractLevelManager> getInstance)
    {
        yield return new WaitUntil(() => getInstance() != null);
        getInstance().Generate();
    }

    public void NextLevel()
    {
        if (_nextBase && !_isTutor)
            AbstractLevelManager.Instance.NextLevel();

        GameStorage.Instance.CollectGameData();
        if (AbstractLevelManager.Instance.isFinalLocation)
        {
            SceneManager.LoadScene("FinalScene");
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (_nextBase)
        {
            SaveLoadManager.Save();
            SceneManager.LoadScene("BaseScene");
        }
        else
            SceneManager.LoadScene("LevelScene");
        _nextBase = _isTutor ? false : !_nextBase;
        _isTutor = false;
    }

    public static void ToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ReloadGame()
    {
        Debug.Log("Reload Game");
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();

        Person.Died.RemoveAllListeners();
        DontDestroyManager.Instance.DestroyAll();

        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        Instance = null;
        //SceneManager.LoadScene(0);
    }
}
