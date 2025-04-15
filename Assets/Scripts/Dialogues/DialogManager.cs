using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, IDialogManager
{
    public static UnityEvent OnAwaked = new UnityEvent();

    [HideInInspector]
    public UnityEvent<Story> OnCreatedStory = new UnityEvent<Story>();


    [SerializeField] DialogDisplayer displayer;

    public static DialogManager Instance { get; private set; }

    public Story CurrentStory { get; private set; }

    bool _storyEnded;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
        OnAwaked?.Invoke();
        displayer.gameObject.SetActive(false);
    }

    public void SetStory(TextAsset inkJSONfile)
    {
        CurrentStory = new Story(inkJSONfile.text);
        OnCreatedStory?.Invoke(CurrentStory);
        _storyEnded = false;
    }

    public void BindFunction(string inkFuncName, Action innerFunc)
    {
        CurrentStory.BindExternalFunction(inkFuncName, innerFunc);
    }

    public void StartStory()
    {
        InputManager.Instance.PushInputMap(InputMap.UI);
        InputManager.Instance.OnSubmitPressed.AddListener(ContinueStory);

        displayer.gameObject.SetActive(true);
        ContinueStory();
    }

    void ContinueStory()
    {
        if (_storyEnded) return;
        if (displayer.IsShowingPhrase)
        {
            displayer.ShowWholePhrase();
            return;
        }

        if (!CurrentStory.canContinue)
        {
            EndDialog();
            return;
        }
        var phrase = CurrentStory.Continue();
        if (string.IsNullOrWhiteSpace(phrase))
        {
            ContinueStory();
            return;
        }
        displayer.ShowPhrase(phrase);

        displayer.HandleTags(CurrentStory.currentTags);

        if (CurrentStory.currentChoices.Count > 0)
            StartCoroutine(WaitShowPhrase());
    }

    IEnumerator WaitShowPhrase()
    {
        yield return new WaitUntil(() => !displayer.IsShowingPhrase);
        var buttons = displayer.CreateChoices(CurrentStory.currentChoices);
        SetChoiceListeners(buttons);
        InputManager.Instance.OnSubmitPressed.RemoveListener(ContinueStory);
    }

    void SetChoiceListeners(List<Button> buttons)
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            var i1 = i;
            buttons[i].onClick.AddListener(() =>
            {
                CurrentStory.ChooseChoiceIndex(i1);
                displayer.DestroyChoices();
                InputManager.Instance.OnSubmitPressed.AddListener(ContinueStory);
                ContinueStory();
            });
        }
    }



    void EndDialog()
    {
        InputManager.Instance.PopInputMap();
        InputManager.Instance.OnSubmitPressed.RemoveListener(ContinueStory);

        displayer.gameObject.SetActive(false);

        _storyEnded = true;
    }

}
