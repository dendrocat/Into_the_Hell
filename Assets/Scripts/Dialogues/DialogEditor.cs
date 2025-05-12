#if UNITY_EDITOR

using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Кастомный редактор для компонента <see cref="DialogManager"/>,
/// интегрирующий окно InkPlayerWindow для удобной работы с Ink-историями в редакторе Unity.
/// </summary>
[CustomEditor(typeof(DialogManager))]
[InitializeOnLoad]
public class DialogEditor : Editor
{
    /// <summary>
    /// Флаг, управляющий свёртыванием/развёртыванием поля истории в инспекторе.
    /// </summary>
    static bool storyExpanded;

    /// <summary>
    /// Статический конструктор, подписывающийся на событие Awake у <see cref="DialogManager"/>.
    /// </summary>
    static DialogEditor()
    {
        DialogManager.Awaked.AddListener(OnAwake);
    }

    /// <summary>
    /// Обработчик события Awake компонента <see cref="DialogManager"/>.
    /// Подписывается на событие создания истории.
    /// </summary>
    static void OnAwake()
    {
        DialogManager.Instance.StoryCreated.AddListener(OnCreateStory);
    }

    /// <summary>
    /// Обработчик события создания истории Ink.
    /// Открывает или обновляет окно InkPlayerWindow для текущей истории.
    /// </summary>
    /// <param name="story">Созданная история Ink.</param>
    static void OnCreateStory(Story story)
    {
        InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        if (window != null) InkPlayerWindow.Attach(story);
    }

    /// <summary>
    /// Отрисовка пользовательского интерфейса инспектора для <see cref="DialogManager"/>.
    /// </summary>
    public override void OnInspectorGUI()
    {
        Repaint();
        base.OnInspectorGUI();
        if (DialogManager.Instance == null) return;
        var story = DialogManager.Instance.CurrentStory;
        InkPlayerWindow.DrawStoryPropertyField(story, ref storyExpanded, new GUIContent("Story"));
    }
}

#endif