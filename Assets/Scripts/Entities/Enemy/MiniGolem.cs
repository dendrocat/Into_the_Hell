// <summary>
/// Класс мини-голема для босса-голма.
/// При смерти снимает эффект MiniGolem с босса.
/// </summary>
public class MiniGolem : BaseEnemy
{
    /// <summary>
    /// Ссылка на босса, к которому относится мини-голем.
    /// </summary>
    [UnityEngine.Tooltip("Ссылка на босса, к которому относится мини-голем")]
    public Boss golemBoss;


    /// <inheritdoc />
    protected override void OnDeath()
    {
        aipath.canMove = false;
        aiDestSetter.target = null;
        golemBoss.RemoveEffect(EffectNames.MiniGolem);
    }
}
