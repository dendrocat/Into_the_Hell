using UnityEngine;

/**
 * <summary>
 * Класс, отвечающий за управление персонажем
 * </summary>
 * **/
public class MovementController : MonoBehaviour
{
    [SerializeField] Player player;
    InputManager inputManager;
    
    /**
     * <summary>
     * Инициализация контроллера
     * </summary>
     * **/
    void Start()
    {
        inputManager = InputManager.Instance;
        if (inputManager == null)
        {
            Debug.LogError(gameObject.name + ": no input manager");
        }
    }

    /**
     * <summary>
     * Обработка ввода игрока
     * </summary>
     * **/
    void Update()
    {
        if (inputManager != null)
        {
            Vector2 movement = inputManager.move;
            player.currentDirection = movement;
            player.setMoving(movement != Vector2.zero);

            if (inputManager.Attack)
            {
                player.Attack();
            }
            if (player.inventory.GetPlayerWeapon() is Sword sword)
            {
                if (inputManager.HoldRightButton != sword.altAttackIsActive())
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
            if (inputManager.Heal)
            {
                Debug.Log("Heal requested");
                player.UsePotion();
            }
        }
    }
}
