using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _nextBase;

    bool _isTutor;

    void OnPlayerDied(Person player)
    {
        if (!(player is Player)) return;
        StartCoroutine(WaitPlayerDestroy(player));
    }

    IEnumerator WaitPlayerDestroy(Person player) {
        yield return new WaitUntil(() => player.IsDestroyed());
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

        GameStorage.Instance.level += Convert.ToInt32(_nextBase && !_isTutor);
        if (GameStorage.Instance.isFirstLevel && _nextBase && !_isTutor)
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

    public void ToMainMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void ReloadGame()
    {
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();

        Person.Died.RemoveAllListeners();
        DontDestroyManager.Instance.DestroyAll();

        SceneManager.LoadScene(1);
    }

    void OnDestroy()
    {
        Instance = null;
        //SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        InputManager.Instance.PushInputMap(InputMap.UI);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        InputManager.Instance.PopInputMap();
    }
}
