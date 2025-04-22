using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player _player;

    [SerializeField] UIController _ui;

    void Start()
    {
        _ui.SetArrows(_player.inventory.GetExplosiveArrowCount());
        _ui.SetMoney(_player.inventory.GetMoney());
        _ui.HealthBar.SetHealth(_player.Health / _player.MaxHealth);
        _ui.ShiftController.SetShiftCount(_player.ShiftCount);
        _ui.PotionController.SetPotions(_player.inventory.GetPotionCount());

        _ui.ChangeWeaponImage(Enum.Parse<WeaponType>(_player.inventory.GetPlayerWeapon().name));

        _player.OnHealthChanged.AddListener(() =>
            _ui.HealthBar.SetHealthSmoothed(_player.Health / _player.MaxHealth)
        );

        _player.inventory.OnPotionCountChanged.AddListener(
            _ui.PotionController.SetPotions
        );

        _player.OnPotionUsed.AddListener(
            _ui.PotionController.StartReload
        );

        _player.OnShiftPerformed.AddListener(() =>
            _ui.ShiftController.SetShiftCount(_player.ShiftCount)
        );
        _player.OnShiftReloadStarted.AddListener(
            _ui.ShiftController.StartShiftSmoothReload
        );

        _player.inventory.OnExplosiveArrowCountChanged.AddListener(_ui.SetArrows);

        _player.inventory.OnMoneyChanged.AddListener(_ui.SetMoneySmooth);

    }
}
