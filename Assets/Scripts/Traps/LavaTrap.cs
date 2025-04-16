using UnityEngine;

public class LavaTrap : BaseTrap
{
    protected override void OnStay(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is in the trap");
        if (target != null)
        {
            target.TakeDamage(50f, DamageType.Fire);
            target.AddEffect(EffectNames.Burning);
        }
    }
}
