using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] protected List<string> targetTags;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float damageRadius = 0.5f;
    protected Rigidbody2D rb;
    public Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /**
     * <summary>
     * Установка урона и скорости снаряда
     * </summary>
     * **/
    public void SetValues(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    void Update()
    {
        Move();
    }

    /**
     * <summary>
     * Метод движения снаряда.
     * </summary>
     * **/
    void Move()
    {
        rb.linearVelocity = (Vector3)direction.normalized * speed;
    }

    /**
     * <summary>
     * Обработка столкновения снаряда с объектом
     * </summary>
     * **/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius).ToList<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (!targetTags.Contains(collider.gameObject.tag)) continue;
            if (damagable != null)
            {
                damagable.TakeDamage(damage, DamageType.None);
            }
        }
        Destroy(gameObject);
    }
}
