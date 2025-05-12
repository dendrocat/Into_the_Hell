using UnityEngine;

/// <summary>
/// Класс, описывающий данные для спавна группы врагов.
/// </summary>
public class SpawnData
{
    /// <summary>
    /// Количество врагов для спавна.
    /// </summary>
    [Tooltip("Количество врагов для спавна")]
    [SerializeField] public readonly int count;

    /// <summary>
    /// Процент здоровья, при котором происходит спавн.
    /// Значение в диапазоне от 0 до 1 (например, 0.5 = 50% здоровья).
    /// </summary>
    [Tooltip("Процент здоровья, при котором происходит спавн (0..1)")]
    [SerializeField, Range(0f, 1f)] public readonly float healthPercentage;

    /// <summary>
    /// Флаг, указывающий, был ли уже выполнен спавн для этих данных.
    /// </summary>
    [Tooltip("Флаг, указывающий, был ли уже выполнен спавн")]
    [SerializeField] public bool passed;

    /// <summary>
    /// Конструктор класса SpawnData.
    /// </summary>
    /// <param name="count">Количество врагов для спавна.</param>
    /// <param name="healthPercentage">Процент здоровья для спавна (0..1).</param>
    public SpawnData(int count, float healthPercentage)
    {
        this.count = count;
        this.healthPercentage = healthPercentage;
        passed = false;
    }
}