using UnityEngine;

/**
 * <summary>
 * Класс, описывающий огненную сферу.
 * </summary>
 * **/
public class FireSphere : Missile
{
    public FireSphere linkedSphere;
    /**
     * <summary>
     * Инициализация огненной сферы
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
            damagable.TakeDamage(damage, DamageType.None);
            Effectable effectable = collider.GetComponent<Effectable>();
            if (effectable)
                effectable.AddEffect(EffectNames.Stun);
        }
        Destroy(linkedSphere.gameObject); // сначала уничтожаем связанную сферу
        Destroy(gameObject);
    }
}
