using Pathfinding;
using UnityEngine;

public class MiniGolem : BaseEnemy
{
    public Boss golemBoss;
    /**
     * <inheritdoc/>
     * **/
    protected override void OnDeath()
    {
        aipath.canMove = false;
        aiDestSetter.target = null;
        golemBoss.RemoveEffect(EffectNames.MiniGolem);
    }
}
