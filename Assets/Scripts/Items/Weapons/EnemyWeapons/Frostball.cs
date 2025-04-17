using UnityEngine;

public class Frostball : Missile
{
    /**
     * <summary>
     * Инициализация ледяного шара
     * </summary>
     * **/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageRadius = 0.01f;
    }

    /**
     * <inheritdoc/>
     * **/
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
