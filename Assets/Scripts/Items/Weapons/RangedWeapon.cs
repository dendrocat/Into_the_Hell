using System.Collections.Generic;
using UnityEngine;

//����� ������ �������� ���
public class RangedWeapon : BaseWeapon
{
    public float range;
    public float angle;

    protected override void Attack() //���������� ������ �����
    {
        List<Person> targets = new List<Person>(); //����� ����� ��� �����
        Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);
        
        foreach(Person candidate in possibleTargets) { // ������� ��������� �����
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

        //��������� ����� ���� �����
        foreach(Person target in targets)
        {
            target.TakeDamage(getScaledDamage());
        }
    }
}
