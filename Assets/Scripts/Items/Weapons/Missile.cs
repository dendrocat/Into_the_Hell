using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public List<string> targetTags;
    public float damage;
    public float speed;
    public float damageRadius = 0.5f;
    public Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = transform.position + (Vector3) direction.normalized * speed * Time.deltaTime;
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
        Destroy(this);
    }
}
