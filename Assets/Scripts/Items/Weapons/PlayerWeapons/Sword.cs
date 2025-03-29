using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий оружие игрока "Меч"
 * </summary>
 * **/
public class Sword : RangedWeapon
{
    bool altAttackActive = false;

    /**
     * <summary>
     * Инициализация оружия
     * </summary>
     * **/
    void Start()
    {
        damage = 7f;
        baseReloadTime = 1f;
        basePrepareAttackTime = 0f;
        baseEndAttackTime = 0f;
        altDamage = 0f;
        baseAltPrepareAttackTime = 1f;
        baseAltEndAttackTime = 1f;
        baseAltReloadTime = 0f;
        scaleCoeff = 1f;
        range = 2f;
        angle = 60f;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack()
    {
        List<IDamagable> targets = FindTargetsForAttack();

        //нанесение урона всем целям
        foreach (IDamagable target in targets)
        {
            target.TakeDamage(getScaledDamage(), DamageType.None);
            /*if (target is Effectable effectableTarget)
            {
                effectableTarget.AddEffect(EffectNames.Freezing);
            }*/
        }
    }

    /**
     * <summary>
     * Активирует или деактивирует альтернативную атаку.
     * </summary>
     * **/
    public override void LaunchAltAttack()
    {
        altAttackActive = !altAttackActive;
        if (altAttackActive) AltAttack();
        else
        {
            EndAltAttack();
        }
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void AltAttack()
    {
        owner.SetEffect(EffectNames.ShieldBlock, level);
    }

    /**
     * <summary>
     * Метод, выполняющий действия по завершению альтернативной атаки оружия
     * </summary>
     * **/
    protected void EndAltAttack()
    {
        owner.SetEffect(EffectNames.ShieldBlock, 0);
    }

    /**
     * <summary>
     * Возвращает, активна ли альтернативная атака.
     * </summary>
     * <returns>true, если альтернативная атака сейчас активна.</returns>
     * **/
    public bool altAttackIsActive()
    {
        return altAttackActive;
    }
}
