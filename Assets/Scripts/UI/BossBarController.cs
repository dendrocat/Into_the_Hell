using UnityEngine;

public class BossBarController : MonoBehaviour
{
    [SerializeField] BossHealthBarController _bossHealthBar;

    [SerializeField] EffectBarController _effect;

    void SetBossEffect(Boss boss)
    {
        EffectNames effect = boss.bossWeapon is YetiWeapon ?
                            EffectNames.AttackChain : EffectNames.MiniGolem;
        _effect.SetGetEffectCount(() =>
            boss.getEffectCount(effect)
        );
        _effect.SetEffectDuration(boss.getEffectDuration(effect));
    }

    public void SetBoss(Boss boss)
    {
        _bossHealthBar.SetBossName(boss.GetBossName());
        Debug.Log($"{boss.GetBossName()} {boss.getHP()} {boss.MaxHealth}");
        _bossHealthBar.SetHealth(boss.getHP(), boss.MaxHealth);
        boss.OnHealthChanged.AddListener(() =>
            _bossHealthBar.SetHealthSmoothed(boss.getHP(), boss.MaxHealth)
        );
        boss.OnDied.AddListener((boss) =>
            gameObject.SetActive(false)
        );
        _bossHealthBar.SetPredicateColorChange(() =>
            boss.hasEffect(EffectNames.MiniGolem)
        );
        SetBossEffect(boss);
    }
}
