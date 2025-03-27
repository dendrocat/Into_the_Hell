using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Класс оружия ближнего боя
public class RangedWeapon : AlternateAttackWeapon
{
    public float range;
    public float angle;

    private void OnDrawGizmos()
    {
        Vector2 ownerPosition = owner.transform.position;
        Vector2 leftAngle = Quaternion.Euler(0f, 0f, angle / 2) * owner.weaponDirection.normalized;
        Vector2 rightAngle = Quaternion.Euler(0f, 0f, -angle / 2) * owner.weaponDirection.normalized;
        Debug.DrawRay(ownerPosition, leftAngle * range, Color.yellow);
        Debug.DrawRay(ownerPosition, rightAngle * range, Color.yellow);
    }

    protected List<IDamagable> FindTargetsForAttack()
    {
        List<IDamagable> targets = new List<IDamagable>(); //поиск целей для атаки
        //Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);

        List<Collider2D> possibleTargets = Physics2D.OverlapCircleAll(owner.transform.position, range).ToList<Collider2D>();

        foreach (Collider2D candidate in possibleTargets)
        { // перебор возможных целей
            IDamagable damagable = candidate.GetComponent<IDamagable>();
            if (!owner.targetTags.Contains(candidate.gameObject.tag)) continue; //не реагируем на цели, не соответствующие тегу

            Vector2 candidatePosition = candidate.transform.position;
            Vector2 candidateDirection = candidatePosition - (Vector2)transform.position;

            if (candidateDirection.magnitude <= range)
            {
                float candidateAngle = Vector2.Angle(candidateDirection, owner.weaponDirection);
                if (candidateAngle <= angle / 2)
                {
                    targets.Add(damagable);
                }
            }
        }

        return targets;
    }

    protected override void Attack() //реализация метода атаки
    {
        List<IDamagable> targets = FindTargetsForAttack();

        //нанесение урона всем целям
        foreach(IDamagable target in targets)
        {
            target.TakeDamage(getScaledDamage(), DamageType.None);
        }
    }
}
