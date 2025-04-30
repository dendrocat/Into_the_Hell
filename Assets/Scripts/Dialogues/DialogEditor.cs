using System.Collections;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogManager))]
[InitializeOnLoad]
public class DialogEditor : Editor
{
    static bool storyExpanded;

    static DialogEditor()
    {
        DialogManager.OnAwaked.AddListener(OnAwake);
    }

    static void OnAwake()
    {
        DialogManager.Instance.OnCreatedStory.AddListener(OnCreateStory);
    }


    static void OnCreateStory(Story story)
    {
        InkPlayerWindow window = InkPlayerWindow.GetWindow(true);
        if (window != null) InkPlayerWindow.Attach(story);
    }

    public override void OnInspectorGUI()
    {
        Repaint();
        base.OnInspectorGUI();
        if (DialogManager.Instance == null) return;
        var story = DialogManager.Instance.CurrentStory;
        InkPlayerWindow.DrawStoryPropertyField(story, ref storyExpanded, new GUIContent("Story"));
    }
}