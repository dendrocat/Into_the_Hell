using System.Collections.Generic;
using UnityEngine;

// ласс оружи€ ближнего бо€
public class RangedWeapon : AlternateAttackWeapon
{
    public float range;
    public float angle;

    private void OnDrawGizmos()
    {
        Vector2 ownerPosition = owner.transform.position;
        Vector2 leftAngle = Quaternion.Euler(0f, 0f, angle / 2) * owner.facingDirection.normalized;
        Vector2 rightAngle = Quaternion.Euler(0f, 0f, -angle / 2) * owner.facingDirection.normalized;
        Debug.DrawRay(ownerPosition, leftAngle * range, Color.yellow);
        Debug.DrawRay(ownerPosition, rightAngle * range, Color.yellow);
    }

    protected override void Attack() //реализаци€ метода атаки
    {
        List<Person> targets = new List<Person>(); //поиск целей дл€ атаки
        Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);
        
        foreach(Person candidate in possibleTargets) { // перебор возможных целей
            if (candidate == owner) continue; //на себ€ не реагируем

            Vector2 candidatePosition = candidate.transform.position;
            Vector2 candidateDirection = candidatePosition - (Vector2) transform.position;

            if (candidateDirection.magnitude <= range)
            {
                float candidateAngle = Vector2.Angle(candidateDirection, owner.facingDirection);
                if (candidateAngle <= angle / 2)
                {
                    targets.Add(candidate);
                }
            }
        }

        //нанесение урона всем цел€м
        foreach(Person target in targets)
        {
            target.TakeDamage(getScaledDamage(), DamageType.None);
        }
    }
}
