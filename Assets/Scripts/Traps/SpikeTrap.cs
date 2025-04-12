using UnityEngine;

public class SpikeTrap : BaseTrap
{
    [SerializeField] float damage;

    void MoveTargetOutsideTrap(Person person)
    {
        Vector2 direction = person.transform.position - transform.position;
        direction = direction.normalized;
        direction = ((direction != Vector2.zero) ? direction : Vector2.right) * 1.5f;

        person.transform.position = transform.position + (Vector3)direction;
    }

    protected override void OnEnter(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is entered the trap");
        if (target != null)
        {
            target.TakeDamage(damage, DamageType.None);
            MoveTargetOutsideTrap(target);
        }
    }

    protected override void OnStay(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is in the trap");
        if (target != null)
        {
            target.TakeDamage(damage, DamageType.None);
            MoveTargetOutsideTrap(target);
        }
    }

    protected override void OnActivate()
    {
        CheckTargetsInTrap();
    }
}
