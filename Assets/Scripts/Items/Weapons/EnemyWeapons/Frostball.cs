using UnityEngine;

/// <summary>
/// Класс, описывающий снаряд "Frostball" с эффектом заморозки.
/// </summary>
public class Frostball : Missile
{
    /// <summary>
    /// Инициализация компонента Rigidbody2D и установка радиуса урона.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageRadius = 0.01f;
    }

    /// <inheritdoc />
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamagable damagable = collider.GetComponent<IDamagable>();
        if ((damagable != null) && targetTags.Contains(collider.gameObject.tag))
        {
            damagable.TakeDamage(damage, DamageType.Fire);
            Effectable effectable = collider.GetComponent<Effectable>();
            if (effectable)
                effectable.AddEffect(EffectNames.Freezing);
        }
        Destroy(gameObject);
    }
}
