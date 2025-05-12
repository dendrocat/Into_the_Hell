using UnityEngine;

/// <summary>
/// Базовый класс для UI-панелей, управляющий состоянием и взаимодействием с инвентарём игрока.
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// Ссылка на инвентарь игрока.
    /// </summary>
    protected PlayerInventory _inventory;

    /// <summary>
    /// Инициализация ссылки на инвентарь при активации панели.
    /// </summary>
    void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    /// <summary>
    /// Очистка ссылки на инвентарь при деактивации панели.
    /// </summary>
    void OnDisable()
    {
        _inventory = null;
    }

    /// <summary>
    /// Открывает панель и переключает карту ввода на UI.
    /// </summary>
    public void Open()
    {
        InputManager.Instance.PushInputMap(InputMap.UI);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Закрывает панель и восстанавливает предыдущую карту ввода.
    /// </summary>
    public void Close()
    {
        InputManager.Instance.PopInputMap();
        gameObject.SetActive(false);
    }
}
