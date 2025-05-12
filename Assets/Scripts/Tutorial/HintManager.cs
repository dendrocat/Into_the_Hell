using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Менеджер подсказок, который отображает список подсказок по очереди,
/// приостанавливая игру и блокируя ввод вне UI.
/// </summary>
public class HintManager : MonoBehaviour
{
    /// <summary>
    /// Singleton-экземпляр менеджера подсказок.
    /// </summary>
    public static HintManager Instance { get; private set; }

    [Tooltip("Компонент для отображения подсказок")]
    [SerializeField] DialogDisplayer displayer;

    List<string> _hints;
    int _index;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            Destroy(gameObject);    
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Запускает показ подсказок по очереди.
    /// </summary>
    /// <param name="hints">Список подсказок для отображения.</param>
    public void ShowHints(List<string> hints)
    {
        _index = 0;
        _hints = hints;

        Time.timeScale = 0;
        InputManager.Instance.PushInputMap(InputMap.UI);
        InputManager.Instance.SubmitPressed.AddListener(ShowNextHint);

        displayer.gameObject.SetActive(true);
        ShowNextHint();
    }

    /// <summary>
    /// Отображает следующую подсказку из списка.
    /// Если текущая подсказка еще печатается, показывает её полностью.
    /// Если подсказки закончились - закрывает окно подсказок.
    /// </summary>
    void ShowNextHint()
    {
        if (displayer.IsShowingPhrase)
        {
            displayer.ShowWholePhrase();
            return;
        }
        if (_index >= _hints.Count)
        {
            CloseHint();
            return;
        }
        displayer.ShowPhrase(_hints[_index]);
        ++_index;
    }

    /// <summary>
    /// Закрывает окно подсказок, восстанавливает управление и время.
    /// </summary>
    void CloseHint()
    {
        InputManager.Instance.PopInputMap();
        InputManager.Instance.SubmitPressed.RemoveListener(ShowNextHint);

        displayer.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

}
