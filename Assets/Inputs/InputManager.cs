using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InputMap
{
    Gameplay, UI
}


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public UnityEvent InteractPressed { get; } = new UnityEvent();

    public UnityEvent SubmitPressed { get; } = new UnityEvent();

    public UnityEvent CancelPressed { get; } = new UnityEvent();

    Stack<InputMap> stackInputs;

    private PlayerInput _playerInput;

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

    public Vector2 move { get; private set; }

    private bool _attack = false;
    public bool Attack => _attack ? (!(_attack = false)) : false;

    private bool _altAttack = false;
    public bool AltAttack => _altAttack ? (!(_altAttack = false)) : false;

    private bool _dash = false;
    public bool Dash => _dash ? (!(_dash = false)) : false;

    private bool _heal = false;
    public bool Heal => _heal ? (!(_heal = false)) : false;

    private bool _pause = false;
    public bool Pause => _pause ? (!(_pause = false)) : false;

    public bool HoldRightButton = false;

    public void onMovePressed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void onAttackPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
        }
    }


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

    public void onInteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPressed?.Invoke();
        }
    }

    public void onDashPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _dash = true;
        }
    }


    public void onHealPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _heal = true;
        }
    }

    public void onPausePressed(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        if (context.performed)
        {
            _pause = true;
        }
    }

    public void onCancelPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CancelPressed.Invoke();
        }
    }

    public void onSubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            SubmitPressed?.Invoke();
    }

    public InputActionAsset GetActions()
    {
        return _playerInput.actions;
    }

    void SwitchInputMap(InputMap map)
    {
        _playerInput.SwitchCurrentActionMap(Enum.GetName(typeof(InputMap), map));
    }

    public void PushInputMap(InputMap map)
    {
        stackInputs.Push(map);
        SwitchInputMap(map);
    }

    public void PopInputMap()
    {
        if (stackInputs.Count > 1)
        {
            stackInputs.Pop();
            SwitchInputMap(stackInputs.Peek());
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
