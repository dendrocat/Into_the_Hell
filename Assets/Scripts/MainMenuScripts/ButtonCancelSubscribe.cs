using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент, который подписывает кнопку на событие отмены из InputManager.
/// При нажатии кнопки отмены вызывается событие onClick кнопки.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonCancelSubscribe : MonoBehaviour
{
    // <summary>
    /// Ссылка на компонент Button.
    /// </summary>
    Button _button;

    /// <summary>
    /// Инициализация ссылки на кнопку.
    /// </summary>
    void Awake()
    {
        _button = GetComponent<Button>();
    }

    /// <summary>
    /// Подписывается на событие <see cref="InputManager.CancelPressed"/> при активации объекта.
    /// </summary>
    void OnEnable()
    {
        InputManager.Instance.CancelPressed.AddListener(_button.onClick.Invoke);
    }

    /// <summary>
    /// Отписывается от события <see cref="InputManager.CancelPressed"/> при деактивации объекта.
    /// </summary>
    void OnDisable()
    {
        InputManager.Instance?.CancelPressed.RemoveListener(_button.onClick.Invoke);
    }
}
