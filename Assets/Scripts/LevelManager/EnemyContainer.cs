using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контейнер для префабов врагов и босса.
/// Этот класс является ScriptableObject и может быть создан через Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "EnemyContainer", menuName = "Containers/EnemyContainer")]
public class EnemyContainer : ScriptableObject
{
    /// <summary>
    /// Список префабов обычных врагов.
    /// </summary>
    [Tooltip("Список префабов обычных врагов")]
    [SerializeField] List<GameObject> _enemies;

    /// <summary>
    /// Префаб босса.
    /// </summary>
    [Tooltip("Префаб босса")]
    [SerializeField] GameObject _boss;

    /// <summary>
    /// Список префабов обычных врагов.
    /// </summary>
    public List<GameObject> Enemies => _enemies;

    /// <summary>
    /// Префаб босса.
    /// </summary>
    public GameObject Boss => _boss;
}
