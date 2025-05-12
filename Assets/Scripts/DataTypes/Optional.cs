using System;
using UnityEngine;

/// <summary>
/// Сериализуемая структура для представления опционального значения.
/// </summary>
/// <typeparam name="T">Тип опционального значения.</typeparam>
/// <remarks>
/// Позволяет легко создавать в инспекторе Unity поля с чекбоксом "включено/выключено".
/// </remarks>
[Serializable]
public struct Optional<T>
{
    /// <summary>
    /// Флаг, указывающий включено ли опциональное значение.
    /// </summary>
    [SerializeField] private bool _enabled;

    /// <summary>
    /// Значение опционального поля.
    /// </summary>
    [SerializeField] private T _value;

    /// <summary>
    /// Выдает или задаёт значение флага активности.
    /// </summary>
    public bool Enabled { get => _enabled; set => _enabled = value; }

    /// <summary>
    /// Выдает значение опционального поля.
    /// </summary>
    public T Value => _value;
}
