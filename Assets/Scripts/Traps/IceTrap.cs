using UnityEngine;

public class IceTrap : BaseTrap
{
    protected override void OnEnter(Person target)
    {
        Debug.Log(gameObject.name + ": target " + target.name + " is in the trap");
        if (target != null)
        {
            target.AddEffect(EffectNames.IceDrifting);
        }
    }

    protected override void OnExit(Person target)
    {
        if (target != null)
        {
            target.RemoveEffect(EffectNames.IceDrifting);
        }
    }
}
