using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Класс, реализующий логику взрыва: визуализацию, нанесение <see cref="DamageType.Explosion">взрывного</see> урона и отбрасывание объектов в радиусе действия.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(Animator), typeof(CircleCollider2D))]
public class Explosion : MonoBehaviour
{
    [Header("Параметры визуализации")]

    /// <summary>
    /// Спрайт взрыва для визуализации.
    /// </summary>
    [Tooltip("Спрайт взрыва для визуализации")]
    [SerializeField] GameObject boomSprite;

    /// <summary>
    /// Коллайдер для определения области взрыва.
    /// </summary>
    CircleCollider2D _collider;


    [Header("Параметры урона")]

    /// <summary>
    /// Урон, наносимый взрывом.
    /// </summary>
    [Tooltip("Урон, наносимый взрывом")]
    [SerializeField] public float _damage;

    /// <summary>
    /// Радиус действия взрыва.
    /// </summary>
    [Tooltip("Радиус действия взрыва")]
    [SerializeField] float damageRadius = 1f;

    /// <summary>
    /// Максимальная сила отбрасывания.
    /// </summary>
    [Tooltip("Максимальная сила отбрасывания")]
    [SerializeField] float magnitude = 50f;

    /// <summary>
    /// Список тегов целей, на которые влияет взрыв.
    /// </summary>
    List<string> _targetTags;

    /// <summary>
    /// Валидация параметров в редакторе.
    /// </summary>
    void OnValidate()
    {
        if (_collider == null)
            _collider = GetComponent<CircleCollider2D>();
        boomSprite.transform.localScale = new Vector3(damageRadius * 2, damageRadius * 2, 0f);
        _collider.radius = damageRadius;
    }

    /// <summary>
    /// Инициализация компонента и уничтожение объекта после анимации.
    /// </summary>
    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = damageRadius;
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.2f);
    }

    /// <summary>
    /// Устанавливает урон взрыва.
    /// </summary>
    /// <param name="damage">Значение урона.</param>
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    /// <summary>
    /// Устанавливает список тегов целей, на которые влияет взрыв.
    /// </summary>
    /// <param name="targetTags">Список тегов целей.</param>
    public void SetTargetTags(List<string> targetTags)
    {
        _targetTags = targetTags;
    }


    /// <summary>
    /// Метод для расчета силы откидывания
    /// </summary>
    /// <param name="range">Расстояние до цели</param>
    /// <param name="maxRange">Максимальное расстояние, на котором влияет взрыв стрелы</param>
    /// <returns>Сила откидывания</returns>
    private float calculateForce(float range, float maxRange)
    {
        return magnitude * Mathf.Cos(Mathf.PI * range / (2 * maxRange)); // m * cos (pi * r / 2R)
    }

    // <summary>
    /// Обработка столкновений - нанесение <see cref="DamageType.Explosion">взрывного</see> урона и отбрасывание объектов в радиусе взрыва.
    /// </summary>
    /// <param name="collision">Информация о столкновении.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius).ToList<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (!_targetTags.Contains(collider.gameObject.tag)) continue;

            //рассчитать направление к цели
            Vector2 targetDirection = collider.gameObject.transform.position - transform.position;

            //рассчитать силу отталкивания
            float force = calculateForce(targetDirection.magnitude, damageRadius);

            if (damagable != null)
            {
                damagable.TakeDamage(_damage, DamageType.Explosion);
                Rigidbody2D rb;
                if (collider.gameObject.TryGetComponent(out rb))
                {
                    rb.AddForce(force * targetDirection, ForceMode2D.Impulse);
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Отрисовка радиуса взрыва в редакторе для удобства настройки.
    /// </summary>
    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, damageRadius);
    }
#endif
}
