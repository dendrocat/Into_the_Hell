using UnityEngine;

public class SpikeTrap : BaseTrap
{
    [SerializeField] float damage;

    protected override void OnEnter(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is entered the trap");
        if (target != null)
        {
            if (!target.hasEffect(EffectNames.Shift))
            {
                target.TakeDamage(damage, DamageType.None);
            }
        }
    }

    protected override void OnStay(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is in the trap");
        if (target != null)
        {
            if (!target.hasEffect(EffectNames.Shift))
            {
                target.TakeDamage(damage, DamageType.None);
            }
        }
    }

    protected override void OnActivate()
    {
        CheckTargetsInTrap();
    }
}
