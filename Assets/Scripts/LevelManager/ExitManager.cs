using UnityEngine;

/// <summary>
/// Менеджер выхода из уровня.
/// Отвечает за создание объекта выхода в заданной позиции.
/// Использует паттерн Singleton для глобального доступа.
/// </summary>
public class ExitManager : MonoBehaviour
{
    /// <summary>
    /// Экземпляр менеджера выхода (Singleton).
    /// </summary>
    public static ExitManager Instance { get; private set; }

    /// <summary>
    /// Префаб объекта выхода.
    /// </summary>
    [Tooltip("Префаб объекта выхода")]
    [SerializeField] GameObject _exit;

    /// <summary>
    /// Инициализация Singleton.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Создаёт объект выхода на заданной позиции.
    /// </summary>
    /// <param name="exitPosition">Позиция для создания выхода.</param>
    public void SpawnExit(Vector3 exitPosition)
    {
        Instantiate(_exit, exitPosition, Quaternion.identity);
    }
}
