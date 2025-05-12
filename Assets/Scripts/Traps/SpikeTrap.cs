using UnityEngine;

/// <summary>
/// Ловушка с шипами, наносящая урон при входе и нахождении цели в зоне ловушки,
/// если у цели отсутствует эффект <see cref="EffectNames.Shift"/> .
/// </summary>
public class SpikeTrap : BaseTrap
{
    [Tooltip("Урон, наносимый шипами")]
    [SerializeField] float damage;

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override void OnActivate()
    {
        CheckTargetsInTrap();
    }
}
