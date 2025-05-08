using UnityEngine;
using UnityEngine.Analytics;

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
        Debug.Log($"BossBar: boss setted");
        _bossHealthBar.SetBossName(boss.GetBossName());
        _bossHealthBar.SetHealth(boss.getHP(), boss.MaxHealth);
        boss.HealthChanged.AddListener(() =>
        {
            Debug.Log("Boss bar: health changed");
            _bossHealthBar.SetHealthSmoothed(boss.getHP(), boss.MaxHealth);
        }
        );
        Person.Died.AddListener(OnBossDied);
        _bossHealthBar.SetPredicateColorChange(() =>
            boss.hasEffect(EffectNames.MiniGolem)
        );
        SetBossEffect(boss);
    }
    void OnDestroy()
    {
        Debug.Log($"Boss bar: destroyed");
        Person.Died.RemoveListener(OnBossDied);
    }

    void OnBossDied(Person person)
    {
        if (!(person is Boss)) return;
        Debug.Log($"BossBar: boss died");
        gameObject.SetActive(false);
    }
}
