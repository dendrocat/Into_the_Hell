using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий стрелу.
 * </summary>
 * **/
public class Arrow : Missile
{
    /**
     * <summary>
     * Инициализация стрелы
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
        }
        Destroy(gameObject);
    }
}
