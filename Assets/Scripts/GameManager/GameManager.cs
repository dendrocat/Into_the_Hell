using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Менеджер игры, реализующий паттерн Singleton.
/// Отвечает за загрузку уровней, управление состоянием игры, паузу и перезагрузку.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Статический экземпляр менеджера (Singleton).
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Флаг, указывающий, что следующий уровень - база.
    /// </summary>
    bool _nextBase;

    /// <summary>
    /// Флаг, указывающий, что игра находится в режиме обучения (туториал).
    /// </summary>
    bool _isTutor;

    /// <summary>
    /// Обработчик события смерти игрока.
    /// Запускает корутину ожидания уничтожения объекта игрока.
    /// </summary>
    /// <param name="person">Объект <see cref="Person">персонажа</see>, который умер.</param>
    void OnPlayerDied(Person person)
    {
        if (!(person is Player player)) return;
        StartCoroutine(WaitPlayerDestroy(player));
    }

    /// <summary>
    /// Корутина, ожидающая уничтожения объекта игрока, после чего перезагружает игру.
    /// </summary>
    /// <param name="player">Объект <see cref="Player">игрока</see>.</param>
    IEnumerator WaitPlayerDestroy(Player player) {
        yield return new WaitUntil(() => player.IsDestroyed());
        ReloadGame();
    }

    /// <summary>
    /// Инициализация Singleton и подписка на событие смерти персонажа.
    /// </summary>
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

    /// <summary>
    /// Загрузка начального уровня (базы или туториала).
    /// </summary>
    /// <param name="isBase">Если true - загружается база, иначе - туториал.</param>
    void LoadStartLevel(bool isBase)
    {
        InputManager.Instance.PushInputMap(InputMap.Gameplay);
        _isTutor = _nextBase = !isBase;
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (isBase) SceneManager.LoadScene("BaseScene");
        else SceneManager.LoadScene("TutorialScene");
    }

    /// <summary>
    /// Запуск новой игры с выбором режима (туториал или нет).
    /// </summary>
    /// <param name="tutorial">Если true - запускается туториал, иначе - обычная игра.</param>
    public void NewGame(bool tutorial)
    {
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();
        SaveLoadManager.Load();
        if (!tutorial) LoadStartLevel(isBase: true);
        else LoadStartLevel(isBase: false);
    }

    /// <summary>
    /// Загрузка сохранённой игры.
    /// </summary>
    public void LoadGame()
    {
        SaveLoadManager.Load();
        LoadStartLevel(isBase: true);
    }

    /// <summary>
    /// Обработчик события загрузки сцены.
    /// Инициализирует игровые данные и запускает генерацию уровня.
    /// </summary>
    /// <param name="scene">Загруженная сцена.</param>
    /// <param name="mode">Режим загрузки сцены.</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameStorage.Instance.InstallGameData();
        StartCoroutine(WaitInstance(() => AbstractLevelManager.Instance));
    }

    /// <summary>
    /// Корутина, ожидающая появления экземпляра <see cref="AbstractLevelManager"/> и запускающая генерацию уровня.
    /// </summary>
    /// <param name="getInstance">Функция получения экземпляра <see cref="AbstractLevelManager"/>.</param>
    IEnumerator WaitInstance(Func<AbstractLevelManager> getInstance)
    {
        yield return new WaitUntil(() => getInstance() != null);
        getInstance().Generate();
    }

    /// <summary>
    /// Переход на следующий уровень или финальную сцену.
    /// </summary>
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

    /// <summary>
    /// Переход в главное меню.
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Переход в главное меню из паузы с возвратом управления.
    /// </summary>
    public void ToMainMenuFromPause() {
        InputManager.Instance.PopInputMap();
        ToMainMenu();
    }

    /// <summary>
    /// Перезагрузка игры: удаление сохранений, очистка слушателей и объектов, загрузка начальной сцены.
    /// </summary>
    public void ReloadGame()
    {
        if (SaveLoadManager.HasSave())
            SaveLoadManager.RemoveSave();

        Person.Died.RemoveAllListeners();
        DontDestroyManager.Instance.DestroyAll();

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Очистка Singleton при уничтожении объекта.
    /// </summary>
    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    /// <summary>
    /// Пауза игры: останавливает время и переключает управление на UI.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        InputManager.Instance.PushInputMap(InputMap.UI);
    }

    // <summary>
    /// Снятие паузы: восстанавливает время и возвращает управление в игру.
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        InputManager.Instance.PopInputMap();
    }
}
