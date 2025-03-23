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
    }

    protected override void Attack() //реализация метода атаки
    {
        List<Person> targets = new List<Person>(); //поиск целей для атаки
        Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);

        foreach (Person candidate in possibleTargets)
        { // перебор возможных целей
            if (candidate == owner) continue; //на себя не реагируем

            Vector2 candidatePosition = candidate.transform.position;
            Vector2 candidateDirection = candidatePosition - (Vector2)transform.position;

            if (candidateDirection.magnitude <= range)
            {
                float candidateAngle = Vector2.Angle(candidateDirection, owner.currentDirection);
                if (candidateAngle <= angle / 2)
                {
                    targets.Add(candidate);
                }
            }
        }

        //нанесение урона всем целям
        foreach (Person target in targets)
        {
            target.TakeDamage(getScaledDamage(), DamageType.None);
            target.AddEffect(EffectNames.Burning);
            target.AddEffect(EffectNames.Freezing);
        }
    }

    public void LaunchAltAttack(bool state) //Активирует или деактивирует альт. атаку
    {
        altAttackActive = state;
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
}
