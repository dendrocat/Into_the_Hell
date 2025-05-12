using UnityEngine;

/// <summary>
/// Контроллер паузы игры.
/// Управляет отображением паузы, приостановкой и возобновлением игры,
/// а также переходом в главное меню из паузы.
/// </summary>
public class PauseController : MonoBehaviour
{
    /// <summary>
    /// Инициализация: скрывает объект паузы при загрузке.
    /// </summary>
    void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Вызывается при активации объекта.
    /// Подписывает события ввода для управления паузой.
    /// </summary>
    void OnEnable()
    {
        Activate();
    }

    /// <summary>
    /// Вызывается при деактивации объекта.
    /// Отписывает события ввода для управления паузой.
    /// </summary>
    void OnDisable()
    {
        Deactivate();
    }

    /// <summary>
    /// Включает паузу: отображает UI паузы и приостанавливает игру.
    /// </summary>
    public void PauseGame()
    {
        gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    /// <summary>
    /// Выключает паузу: скрывает UI паузы и возобновляет игру.
    /// </summary>
    public void UnPauseGame()
    {
        gameObject.SetActive(false);
        GameManager.Instance.UnPauseGame();
    }

    /// <summary>
    /// Переход в главное меню из паузы.
    /// Сначала снимает паузу, затем вызывает переход.
    /// </summary>
    public void ToMainMenu()
    {
        UnPauseGame();
        GameManager.Instance.ToMainMenuFromPause();
    }

    /// <summary>
    /// Активирует подписки на события ввода для управления паузой.
    /// Отписывает от события нажатия паузы и подписывается на отмену.
    /// </summary>
    public void Activate()
    {
        InputManager.Instance.PausePressed.RemoveListener(PauseGame);
        InputManager.Instance.CancelPressed.AddListener(UnPauseGame);
    }

    /// <summary>
    /// Деактивирует подписки на события ввода для управления паузой.
    /// Подписывается на событие нажатия паузы и отписывается от отмены.
    /// </summary>
    public void Deactivate()
    {
        InputManager.Instance.PausePressed.AddListener(PauseGame);
        InputManager.Instance.CancelPressed.RemoveListener(UnPauseGame);
    }
}
