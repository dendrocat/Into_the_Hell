using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A serializable structure that represents an optional value.
/// </summary>
/// <typeparam name="T">The type of the optional value.</typeparam>
[Serializable]
public struct Optional<T>
{
    /// <summary>
    /// Indicates whether the optional value is enabled.
    /// </summary>
    [SerializeField] private bool _enabled;

    /// <summary>
    /// The value of the optional field.
    /// </summary>
    [SerializeField] private T _value;

    /// <summary>
    /// Gets a value indicating whether the optional value is enabled.
    /// </summary>
    public bool Enabled { get => _enabled; set => _enabled = value; }

    /// <summary>
    /// Gets the value of the optional field.
    /// </summary>
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
