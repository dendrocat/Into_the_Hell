using UnityEngine;

public class SpikeTrap : BaseTrap
{
    [SerializeField] float damage;

    protected override void OnEnter(Person target)
    {
        if (target != null)
        {
            target.TakeDamage(damage, DamageType.None);
        }
    }
}
