using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosiveArrow : Missile
{
    [SerializeField] float magnitude = 50f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageRadius = 1f;
    }

    private float calculateForce(float range, float maxRange)
    {
        return magnitude * Mathf.Cos(Mathf.PI * range / (2 * maxRange)); // m * cos (pi * r / 2R)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius).ToList<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (!targetTags.Contains(collider.gameObject.tag)) continue;

            //рассчитать направление к цели
            Vector2 targetDirection = collider.gameObject.transform.position - transform.position;

            //рассчитать силу отталкивания
            float force = calculateForce(targetDirection.magnitude, damageRadius);

            if (damagable != null)
            {
                damagable.TakeDamage(damage, DamageType.None);
                Rigidbody2D rb;
                if (collider.gameObject.TryGetComponent<Rigidbody2D>(out rb))
                {
                    rb.AddForce(force * targetDirection, ForceMode2D.Impulse);
                }
            }
        }
        Destroy(gameObject);
    }
}
