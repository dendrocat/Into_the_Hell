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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetValues(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        rb.linearVelocity = (Vector3)direction.normalized * speed;
    }

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
