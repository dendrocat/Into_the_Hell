using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Player _player;

    [SerializeField] UIController _ui;

    [SerializeField] Boss _debugBoss;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        if (_debugBoss == null)
            Debug.LogWarning("Boss was not setted in debug mode");
        else SetBoss(_debugBoss);
    }

    void Start()
    {
        _ui.SetArrows(_player.inventory.GetExplosiveArrowCount());
        _ui.SetMoney(_player.inventory.GetMoney());
        _ui.HealthBar.SetHealth(_player.Health, _player.MaxHealth);
        _ui.ShiftController.SetShiftCount(_player.ShiftCount);
        _ui.PotionController.SetPotions(_player.inventory.GetPotionCount());

        _ui.ChangeWeaponImage(Enum.Parse<WeaponType>(_player.inventory.GetPlayerWeapon().name));

        _player.OnHealthChanged.AddListener(() =>
            _ui.HealthBar.SetHealthSmoothed(
                _player.Health, _player.MaxHealth
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
        _ui.BossHealthBar.SetBossName(boss.GetBossName());
        Debug.Log($"{boss.GetBossName()} {boss.Health} {boss.MaxHealth}");
        _ui.BossHealthBar.SetHealth(boss.Health, boss.MaxHealth);
        boss.OnHealthChanged.AddListener(() =>
            _ui.BossHealthBar.SetHealthSmoothed(boss.Health, boss.MaxHealth)
        );
        boss.OnDied.AddListener(() =>
            _ui.BossHealthBar.gameObject.SetActive(false)
        );
        _ui.BossHealthBar.gameObject.SetActive(true);
    }
}
