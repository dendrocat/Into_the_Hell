using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MissilesAudio))]
public class Missile : MonoBehaviour
{
    /// <summary>
    /// Список тегов целей, по которым снаряд может нанести урон.
    /// </summary>
    [Tooltip("Список тегов целей, по которым снаряд может нанести урон")]
    [SerializeField] protected List<string> targetTags;

    /// <summary>
    /// Урон, наносимый снарядом.
    /// </summary>
    [Tooltip("Урон, наносимый снарядом")]
    [SerializeField] protected float damage;

    // <summary>
    /// Скорость движения снаряда.
    /// </summary>
    [Tooltip("Скорость движения снаряда")]
    [SerializeField] protected float speed;

    /// <summary>
    /// Радиус урона снаряда при столкновении.
    /// </summary>
    [Tooltip("Радиус урона снаряда при столкновении")]
    [SerializeField] protected float damageRadius = 0.5f;

    /// <summary>
    /// Rigidbody2D компонента для физического движения снаряда.
    /// </summary>
    protected Rigidbody2D rb;

    /// <summary>
    /// Направление движения снаряда.
    /// </summary>
    public Vector2 direction;

    /// <summary>
    /// Инициализация компонента Rigidbody2D.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Получение урона, наносимого снарядом
    /// </summary>
    /// <returns><see langword="float"/> - Значение урона.</returns>
    public float GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Получение тегов целей снаряда
    /// </summary>
    /// <returns>Список тегов целей.</returns>
    public List<string> GetTargetTags()
    {
        return targetTags;
    }

    /// <summary>
    /// Установка целей для снаряда
    /// </summary>
    /// <param name="targetTags">Список тегов целей.</param>
    public void SetTargetTags(List<string> targetTags)
    {
        this.targetTags = targetTags;
    }

    /// <summary>
    /// Установка урона и скорости снаряда
    /// </summary>
    /// <param name="damage">Урон снаряда.</param>
    /// <param name="speed">Скорость снаряда.</param>
    public void SetValues(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    /// <summary>
    /// Обновление каждый кадр: движение снаряда.
    /// </summary>
    void Update()
    {
        Move();
    }

    /// <summary>
    /// Метод движения снаряда.
    /// </summary>
    void Move()
    {
        rb.linearVelocity = (Vector3)direction.normalized * speed;
    }

    /// <summary>
    /// Обработка столкновения снаряда с объектом
    /// </summary>
    /// <param name="collision">Информация о столкновении.</param>
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
