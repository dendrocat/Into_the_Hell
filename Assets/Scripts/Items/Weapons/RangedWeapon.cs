using System.Collections.Generic;
using UnityEngine;

//Класс оружия ближнего боя
public class RangedWeapon : BaseWeapon
{
    public float range;
    public float angle;

    protected override void Attack() //реализация метода атаки
    {
        List<Person> targets = new List<Person>(); //поиск целей для атаки
        Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);
        
        foreach(Person candidate in possibleTargets) { // перебор возможных целей
            Vector2 candidatePosition = candidate.transform.position;
            Vector2 candidateDirection = candidatePosition - (Vector2) transform.position;

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
        foreach(Person target in targets)
        {
            target.TakeDamage(getScaledDamage());
        }
    }
}
