using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [SerializeField] DialogDisplayer displayer;

    List<string> _hints;
    int _index;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
    }

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

    void CloseHint()
    {
        InputManager.Instance.PopInputMap();
        InputManager.Instance.SubmitPressed.RemoveListener(ShowNextHint);

        displayer.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
