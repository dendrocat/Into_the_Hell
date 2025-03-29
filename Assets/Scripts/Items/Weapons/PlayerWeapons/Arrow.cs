using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * <summary>
 * �����, ����������� ������.
 * </summary>
 * **/
public class Arrow : Missile
{
    /**
     * <summary>
     * ������������� ������
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
