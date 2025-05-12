using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// Менеджер ввода, реализующий паттерн Singleton для централизованного управления пользовательскими действиями.
/// </summary>
/// <remarks>
/// Класс автоматически требует компонент <see cref="PlayerInput"/> на том же объекте.
/// </remarks>
[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Экземпляр <see cref="InputManager"/> (Singleton).
    /// </summary>
    public static InputManager Instance { get; private set; }

    /// <summary>
    /// Событие, вызываемое при нажатии кнопки взаимодействия (Interact).
    /// </summary>
    public UnityEvent InteractPressed { get; } = new();

    /// <summary>
    /// Событие, вызываемое при нажатии кнопки подтверждения (Submit).
    /// </summary>
    public UnityEvent SubmitPressed { get; } = new();

    /// <summary>
    /// Событие, вызываемое при нажатии кнопки отмены (Cancel).
    /// </summary>
    public UnityEvent CancelPressed { get; } = new();

    /// <summary>
    /// Событие, вызываемое при нажатии кнопки паузы (Pause).
    /// </summary>
    public UnityEvent PausePressed { get; } = new();

    /// <summary>
    /// Стек для управления текущими активными схемами ввода.
    /// </summary>
    Stack<InputMap> stackInputs;

    /// <summary>
    /// Ссылка на компонент PlayerInput.
    /// </summary>
    private PlayerInput _playerInput;

    /// <summary>
    /// Текущая векторная нормализованная величина движения, получаемая из ввода.
    /// </summary>
    public Vector2 move { get; private set; }

    private bool _attack = false;
    /// <summary>
    /// Флаг нажатия основной атаки. Сбрасывается после чтения.
    /// </summary>
    public bool Attack => _attack ? (!(_attack = false)) : false;

    private bool _altAttack = false;
    /// <summary>
    /// Флаг нажатия альтернативной атаки. Сбрасывается после чтения.
    /// </summary>
    public bool AltAttack => _altAttack ? (!(_altAttack = false)) : false;

    private bool _dash = false;
    /// <summary>
    /// Флаг нажатия рывка (dash). Сбрасывается после чтения.
    /// </summary>
    public bool Dash => _dash ? (!(_dash = false)) : false;

    private bool _heal = false;
    /// <summary>
    /// Флаг нажатия лечения. Сбрасывается после чтения.
    /// </summary>
    public bool Heal => _heal ? (!(_heal = false)) : false;

    /// <summary>
    /// Флаг удержания правой кнопки (альтернативной атаки с удержанием).
    /// </summary>
    public bool HoldRightButton = false;

    /// <summary>
    /// Флаг удержания левого Ctrl.
    /// </summary>
    public bool HoldLeftCtrl = false;

    /// <summary>
    /// Инициализация экземпляра и установка дефолтной схемы ввода.
    /// </summary>
    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"{gameObject}: Instance уже существует");
            return;
        }
        Instance = this;
        stackInputs = new();

        _playerInput = GetComponent<PlayerInput>();
        PushInputMap((InputMap)Enum.Parse(typeof(InputMap), _playerInput.currentActionMap.name));
    }

    #region Обработчики ввода

    /// <summary>
    /// Обработчик события движения.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onMovePressed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Обработчик нажатия основной атаки.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onAttackPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
        }
    }

    /// <summary>
    /// Обработчик альтернативной атаки с поддержкой удержания.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onAltAttackPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    HoldRightButton = true;
                    break;
                case InputActionPhase.Canceled:
                    HoldRightButton = false;
                    break;
            }
        }
        else if (context.interaction is PressInteraction)
        {
            if (context.performed)
            {
                _altAttack = true;
            }
        }
    }

    /// <summary>
    /// Обработчик удержания левого Ctrl.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onLeftCtrlPressed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Debug.Log("Left ctrl pressed");
                    HoldLeftCtrl = true;
                    break;
                case InputActionPhase.Canceled:
                    Debug.Log("Left ctrl released");
                    HoldLeftCtrl = false;
                    break;
            }
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки взаимодействия.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onInteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPressed?.Invoke();
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки рывка.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onDashPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _dash = true;
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки лечения.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onHealPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _heal = true;
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки паузы.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onPausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PausePressed.Invoke();
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки отмены.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onCancelPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CancelPressed.Invoke();
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки подтверждения.
    /// </summary>
    /// <param name="context">Контекст действия ввода.</param>
    public void onSubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            SubmitPressed?.Invoke();
    }

    #endregion

    /// <summary>
    /// Получить все действия ввода из PlayerInput.
    /// </summary>
    /// <returns>Объект InputActionAsset с действиями ввода.</returns>
    public InputActionAsset GetActions()
    {
        return _playerInput.actions;
    }

    /// <summary>
    /// Переключить текущую схему ввода на указанную.
    /// </summary>
    /// <param name="map">Новая схема ввода из перечисления <see cref="InputMap"/>.</param>
    void SwitchInputMap(InputMap map)
    {
        _playerInput.SwitchCurrentActionMap(Enum.GetName(typeof(InputMap), map));
    }

    /// <summary>
    /// Добавить новую схему ввода в стек и переключиться на неё.
    /// </summary>
    /// <param name="map">Схема ввода из перечисления <see cref="InputMap"/> для добавления.</param>
    public void PushInputMap(InputMap map)
    {
        stackInputs.Push(map);
        SwitchInputMap(map);
    }

    /// <summary>
    /// Удалить текущую схему ввода из стека и переключиться на предыдущую, если она есть.
    /// </summary>
    public void PopInputMap()
    {
        if (stackInputs.Count > 1)
        {
            stackInputs.Pop();
            SwitchInputMap(stackInputs.Peek());
        }
    }
}
