using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Менеджер диалогов, управляющий процессом отображения и взаимодействия с Ink-историями.
/// </summary>
/// <remarks>
/// Реализует паттерн Singleton для глобального доступа.
/// Обеспечивает загрузку истории, отображение текста, обработку тегов, вариантов выбора и завершение диалога.
/// </remarks>
public class DialogManager : MonoBehaviour, IDialogManager
{
    /// <summary>
    /// Событие, вызываемое при инициализации экземпляра <see cref="DialogManager"/>.
    /// </summary>
    public static UnityEvent Awaked = new();

    /// <summary>
    /// Событие, вызываемое при создании новой Ink-истории.
    /// </summary>
    [HideInInspector]
    public UnityEvent<Story> StoryCreated = new();

    /// <summary>
    /// Событие, вызываемое при окончании Ink-истории.
    /// </summary>
    [HideInInspector]
    public UnityEvent StoryEnded = new();

    // <summary>
    /// Единственный экземпляр <see cref="DialogManager"/> (Singleton).
    /// </summary>
    public static DialogManager Instance { get; private set; }

    [Tooltip("Компонент для отображения диалогов")]
    [SerializeField] DialogDisplayer displayer;

    /// <summary>
    /// Текущая активная Ink-история.
    /// </summary>
    public Story CurrentStory { get; private set; }

    /// <summary>
    /// Инициализация Singleton и скрытие UI диалогов.
    /// </summary>
    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        displayer.gameObject.SetActive(false);
        Awaked?.Invoke();
    }

    /// <inheritdoc />
    public void SetStory(TextAsset inkJSONfile)
    {
        CurrentStory = new Story(inkJSONfile.text);
        StoryCreated?.Invoke(CurrentStory);
    }

    /// <inheritdoc />
    public void BindFunction(string inkFuncName, Action innerFunc)
    {
        CurrentStory.BindExternalFunction(inkFuncName, innerFunc);
    }

    /// <summary>
    /// Запускает диалог, переключая управление на UI и показывая первую фразу.
    /// </summary>
    public void StartStory()
    {
        InputManager.Instance.PushInputMap(InputMap.UI);
        InputManager.Instance.SubmitPressed.AddListener(ContinueStory);

        displayer.gameObject.SetActive(true);
        ContinueStory();
    }

    /// <summary>
    /// Продолжает диалог, показывая следующую фразу или варианты выбора.
    /// </summary>
    void ContinueStory()
    {
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

    /// <summary>
    /// Корутина, ожидающая завершения показа фразы, затем отображающая варианты выбора.
    /// </summary>
    IEnumerator WaitShowPhrase()
    {
        yield return new WaitUntil(() => !displayer.IsShowingPhrase);
        var buttons = displayer.CreateChoices(CurrentStory.currentChoices);
        SetChoiceListeners(buttons);
        InputManager.Instance.SubmitPressed.RemoveListener(ContinueStory);
    }

    /// <summary>
    /// Назначает обработчики нажатий для кнопок выбора.
    /// </summary>
    /// <param name="buttons">Список кнопок выбора.</param>
    void SetChoiceListeners(List<Button> buttons)
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            var i1 = i;
            buttons[i].onClick.AddListener(() =>
            {
                CurrentStory.ChooseChoiceIndex(i1);
                displayer.DestroyChoices();
                InputManager.Instance.SubmitPressed.AddListener(ContinueStory);
                ContinueStory();
            });
        }
    }

    /// <summary>
    /// Завершает диалог, скрывая UI и возвращая управление игроку.
    /// </summary>
    void EndDialog()
    {
        InputManager.Instance.PopInputMap();
        InputManager.Instance.SubmitPressed.RemoveListener(ContinueStory);

        displayer.gameObject.SetActive(false);

        StoryEnded.Invoke();
    }

}
