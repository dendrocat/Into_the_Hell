using System;
using UnityEngine;

/// <summary>
/// Менеджер UI, обеспечивающий связь между игроком и элементами интерфейса.
/// Реализует паттерн Singleton.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Singleton-экземпляр UIManager.
    /// </summary>
    public static UIManager Instance { get; private set; }

    Player _player;
    UIController _ui;

#if UNITY_EDITOR
    [Tooltip("Босс для отладки в редакторе")]
    [SerializeField] Boss debugBoss;
#endif

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Инициализирует UI.
    /// </summary>
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

        _player.HealthChanged.AddListener(() =>
            _ui.HealthBar.SetHealthSmoothed(
                _player.getHP(), _player.MaxHealth
            )
        );

        _player.inventory.PotionCountChanged.AddListener(
            _ui.PotionController.SetPotions
        );

        _player.PotionUsed.AddListener(
            _ui.PotionController.StartReload
        );

        _player.ShiftPerformed.AddListener(() =>
            _ui.ShiftController.SetShiftCount(_player.ShiftCount)
        );
        _player.ShiftReloadStarted.AddListener(
            _ui.ShiftController.StartShiftSmoothReload
        );

        _player.inventory.ExplosiveArrowCountChanged.AddListener(_ui.SetArrows);

        _player.inventory.MoneyChanged.AddListener(_ui.SetMoneySmooth);
#if UNITY_EDITOR
        if (debugBoss) SetBoss(debugBoss);
#endif
    }

    // <summary>
    /// Устанавливает босса для отображения на UI.
    /// </summary>
    /// <param name="boss">Объект босса.</param>
    public void SetBoss(Boss boss)
    {
        _ui.BossBarController.SetBoss(boss);
        _ui.BossBarController.gameObject.SetActive(true);
    }

    /// <summary>
    /// Генерирует мини-карту на основе данных <see cref="Level"/>уровня</see>.
    /// </summary>
    /// <param name="level"><see cref="Level"/>Уровень</see> с информацией о комнатах.</param>
    public void GenerateMap(Level level)
    {
        _ui.MiniMapUI.SetUpLevel(level);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
