using UnityEngine;

/// <summary>
/// Тестовый менеджер игры для запуска генерации уровня при старте сцены.
/// </summary>
public class TestGameManager : MonoBehaviour
{
    /// <summary>
    /// Метод вызывается при старте сцены.
    /// Запускает генерацию уровня через AbstractLevelManager.
    /// </summary>
    void Start()
    {
        AbstractLevelManager.Instance.Generate();
    }
}
