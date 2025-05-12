using UnityEngine;


/// <summary>
/// Класс, описывающий огненный шар.
/// </summary>
public class Fireball : Missile
{
    
    /// <summary>
    /// Инициализация огненного шара
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageRadius = 0.01f;
    }

    /// <summary>
    /// Обработка столкновения с объектом.
    /// Наносит урон и накладывает эффект оглушения, затем уничтожает связанную и текущую сферу.
    /// </summary>
    /// <param name="collision">Информация о столкновении.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamagable damagable = collider.GetComponent<IDamagable>();
        if ((damagable != null) && targetTags.Contains(collider.gameObject.tag))
        {
            damagable.TakeDamage(damage, DamageType.Fire);
            Effectable effectable = collider.GetComponent<Effectable>();
            if (effectable)
                effectable.AddEffect(EffectNames.Burning);
        }
        Destroy(gameObject);
    }
}
