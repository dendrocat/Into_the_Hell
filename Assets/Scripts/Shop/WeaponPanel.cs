using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Панель выбора оружия, наследует <see cref="BasePanel"/>.
/// Управляет отображением доступных оружий и переключением между ними.
/// </summary>
public class WeaponPanel : BasePanel
{
    /// <summary>
    /// Событие, вызываемое при смене оружия.
    /// Передает выбранный тип оружия.
    /// </summary>
    public UnityEvent<WeaponType> WeaponChanged = new();

    [Tooltip("Список кнопок для выбора оружия")]
    [SerializeField] List<Button> _windows;

    void Start()
    {
        var weapon = _inventory.GetPlayerWeapon().GetType().Name;
        _windows[(int)Enum.Parse<WeaponType>(weapon)].interactable = false;
    }

    /// <summary>
    /// Обновляет состояние кнопок, делая выбранную неактивной.
    /// </summary>
    /// <param name="type">Тип выбранного оружия.</param>
    void CalcItemState(WeaponType type)
    {
        foreach (var item in _windows)
        {
            item.interactable = true;
        }
        _windows[(int)type].interactable = false;
    }

    /// <summary>
    /// Меняет текущее оружие игрока по имени типа.
    /// </summary>
    /// <param name="type">Имя типа оружия.</param>
    public void ChangeWeapon(string type)
    {
        if (!Enum.TryParse<WeaponType>(type, out var weaponType))
        {
            Debug.LogError($"WeaponType: {type} doesn't exist");
            return;
        }
        _inventory.SetPlayerWeapon(
            WeaponStorage.Instance.GetWeapon(weaponType)
        );

        CalcItemState(weaponType);
        WeaponChanged.Invoke(weaponType);
    }
}
