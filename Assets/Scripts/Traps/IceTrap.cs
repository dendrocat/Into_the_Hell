using UnityEngine;

/// <summary>
/// Ловушка, накладывающая эффект <see cref="EffectNames.IceDrifting"/> на цель при входе и снимающая при выходе.
/// </summary>
public class IceTrap : BaseTrap
{
    /// <inheritdoc />
    protected override void OnEnter(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is in the trap");
        if (target != null)
        {
            target.AddEffect(EffectNames.IceDrifting);
        }
    }

    /// <inheritdoc />
    protected override void OnExit(Person target)
    {
        if (target != null)
        {
            target.RemoveEffect(EffectNames.IceDrifting);
        }
    }
}
