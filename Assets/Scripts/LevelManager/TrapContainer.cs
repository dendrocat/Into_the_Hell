using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контейнер для настроек ловушек.
/// Этот класс является ScriptableObject и может быть создан через Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "TrapContainer", menuName = "Containers/TrapContainer")]
public class TrapContainer : ScriptableObject
{
    /// <summary>
    /// Полное имя типа ловушки (например, "IceTrap").
    /// </summary>
    [Tooltip("Полное имя типа ловушки (например, \"IceTrap\")")]
    [SerializeField] string _trapTypeName;

    /// <summary>
    /// Период проверки ловушки (в секундах).
    /// </summary>
    [Tooltip("Период проверки ловушки (в секундах)")]
    [SerializeField] float _trapCheckPeriod;

    /// <summary>
    /// Список тегов целей, на которые ловушка реагирует.
    /// </summary>
    [Tooltip("Список тегов целей, на которые ловушка реагирует")]
    [SerializeField] List<string> _targetTags;

    /// <summary>
    /// Тип ловушки по имени.
    /// </summary>
    public Type TrapType => Type.GetType(_trapTypeName);

    /// <summary>
    /// Период проверки ловушки.
    /// </summary>
    public float TrapCheckPeriod => _trapCheckPeriod;

    /// <summary>
    /// Список тегов целей для ловушки.
    /// </summary>
    public List<string> TargetTags => _targetTags;
}
