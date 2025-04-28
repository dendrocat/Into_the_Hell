using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    Player _player;

    UIController _ui;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _ui = FindFirstObjectByType<UIController>();

        _ui.SetArrows(_player.inventory.GetExplosiveArrowCount());
        _ui.SetMoney(_player.inventory.GetMoney());
        _ui.HealthBar.SetHealth(_player.getHP(), _player.MaxHealth);
        _ui.ShiftController.SetShiftCount(_player.ShiftCount);
        _ui.PotionController.SetPotions(_player.inventory.GetPotionCount());

        _ui.ChangeWeaponImage(
            Enum.Parse<WeaponType>(_player.inventory.GetPlayerWeapon().GetType().Name)
        );

        _player.OnHealthChanged.AddListener(() =>
            _ui.HealthBar.SetHealthSmoothed(
                _player.getHP(), _player.MaxHealth
            )
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

    public void SetBoss(Boss boss)
    {
        _ui.BossBarController.SetBoss(boss);
        _ui.BossBarController.gameObject.SetActive(true);
    }
}
