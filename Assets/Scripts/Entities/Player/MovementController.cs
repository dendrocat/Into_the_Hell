using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Player player;
    InputManager inputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        if (inputManager == null)
        {
            Debug.LogError(gameObject.name + ": no input manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager != null)
        {
            Vector2 movement = inputManager.move;
            player.currentDirection = movement;
            player.isMoving = (movement != Vector2.zero);

            if (inputManager.Attack)
            {
                player.Attack();
            }
            if (player.playerWeapon is Sword)
            {
                if (inputManager.HoldRightButton != ((Sword) player.playerWeapon).altAttackIsActive())
                {
                    player.AltAttack();
                }
            }
            else
            {
                if (inputManager.AltAttack)
                {
                    Debug.Log("Alt attack requested");
                    player.AltAttack();
                }
            }
            if (inputManager.Dash)
            {
                player.PerformShift();
            }
        }
    }
}
