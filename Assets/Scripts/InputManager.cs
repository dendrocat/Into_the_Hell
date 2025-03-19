using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Instance уже существует");
            return;
        }
        Instance = this;
    }


    public Vector2 move { get; private set; }

    private bool _attack = false;

    public bool attack => _attack ? (_attack = false) == false : false;



    public void onMovePressed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        Debug.Log(move);
    }

    public void onAttackPressoed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
        }
    }
}
