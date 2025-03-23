using System.Collections.Generic;
using UnityEngine;

//����� ������ ������ "���"
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

    protected override void Attack() //���������� ������ �����
    {
        List<Person> targets = new List<Person>(); //����� ����� ��� �����
        Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);

        foreach (Person candidate in possibleTargets)
        { // ������� ��������� �����
            if (candidate == owner) continue; //�� ���� �� ���������

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

        //��������� ����� ���� �����
        foreach (Person target in targets)
        {
            target.TakeDamage(getScaledDamage(), DamageType.None);
            target.AddEffect(EffectNames.Burning);
            target.AddEffect(EffectNames.Freezing);
        }
    }

    public override void LaunchAltAttack() //���������� ��� ������������ ����. �����
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
