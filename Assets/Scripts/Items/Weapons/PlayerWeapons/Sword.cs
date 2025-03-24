using System.Collections.Generic;
using UnityEngine;

//Класс оружия игрока "Меч"
public class Sword : RangedWeapon
{
    bool altAttackActive = false;
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

    protected override void Attack() //реализация метода атаки
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

    public override void LaunchAltAttack() //Активирует или деактивирует альт. атаку
    {
        altAttackActive = !altAttackActive;
        if (altAttackActive) AltAttack();
        else
        {
            EndAltAttack();
        }
    }

    protected override void AltAttack()
    {
        owner.SetEffect(EffectNames.ShieldBlock, level);
    }

    protected void EndAltAttack()
    {
        owner.SetEffect(EffectNames.ShieldBlock, 0);
    }

    public bool altAttackIsActive()
    {
        return altAttackActive;
    }
}
