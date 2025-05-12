using UnityEngine;

/// <summary>
/// Ловушка лавы, наносящая <see cref="DamageType.Fire">огненный</see> урон и накладывающая эффект <see cref="EffectNames.Burning"/>  на цель, находящуюся в зоне ловушки.
/// </summary>
public class LavaTrap : BaseTrap
{
    /// <inheritdoc />
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
