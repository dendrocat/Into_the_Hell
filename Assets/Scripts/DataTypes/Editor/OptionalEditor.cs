#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/// <summary>
/// Кастомный редактор для отображения структуры <see cref="Optional" /> в инспекторе Unity.
/// </summary>
/// <remarks>
/// Отображает чекбокс включения и поле значения в одной строке.
/// Автоматически отключает поле значения, если чекбокс выключен.
/// </remarks>
[CustomPropertyDrawer(typeof(Optional<>))]
public class OptionalPropertyDrawer : PropertyDrawer
{
    /// <summary>
    /// Отрисовка GUI элемента для опционального поля.
    /// </summary>
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