using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMap
{
    Gameplay, UI
}


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputMap CurrentInputMap { get; private set; }

    private PlayerInput _playerInput;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Instance уже существует");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public Vector2 move { get; private set; }

    private bool _attack = false;
    public bool Attack => _attack ? (!(_attack = false)) : false;

    private bool _altAttack = false;
    public bool AltAttack => _altAttack ? (!(_altAttack = false)) : false;

    private bool _interact = false;
    public bool Interact => _interact ? (!(_interact = false)) : false;

    private bool _dash = false;
    public bool Dash => _dash ? (!(_dash = false)) : false;

    private bool _heal = false;
    public bool Heal => _heal ? (!(_heal = false)) : false;

    private bool _pause = false;
    public bool Pause => _pause ? (!(_pause = false)) : false;

    private bool _cancel = false;
    public bool Cancel => _cancel ? (!(_cancel = false)) : false;

    public void onMovePressed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void onAttackPressoed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
        }
    }


    public void onAltAttackPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _altAttack = true;
        }
    }

    public void onInteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _interact = true;
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
            Debug.Log("Cancel");
            _cancel = true;
        }
    }
    public void SwitchInputMap(InputMap map)
    {
        _playerInput.SwitchCurrentActionMap(Enum.GetName(typeof(InputMap), map));
        CurrentInputMap = map;
    }


}
