using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

[Serializable]
public struct Optional<T>
{
    [SerializeField] bool _enabled;
    [SerializeField] T _value;

    public bool Enabled => _enabled;
    public T Value => _value;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(Optional<>))]
public class OptionalPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var valueProperty = property.FindPropertyRelative("_value");
        var enabledProperty = property.FindPropertyRelative("_enabled");


        const float checkboxWidth = 24f;

        var enabledRect = new Rect(position.x, position.y, checkboxWidth, position.height);
        EditorGUI.PropertyField(enabledRect, enabledProperty, label, true);

        var valueRect = new Rect(
            position.x + EditorGUIUtility.labelWidth + checkboxWidth,
            position.y,
            position.width - EditorGUIUtility.labelWidth - checkboxWidth,
            position.height
        );

        EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
        EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
        EditorGUI.EndDisabledGroup();

    }
}


#endif
