using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator), typeof(CircleCollider2D))]
public class Explosion : MonoBehaviour
{
    [Header("Render parameters")]
    [SerializeField] GameObject boomSprite;
    CircleCollider2D _collider;

    [Header("Damage parameters")]
    [SerializeField] public float _damage;
    [SerializeField] float damageRadius = 1f;
    [SerializeField] float magnitude = 50f;

    [SerializeField] float destructionTime;

    List<string> _targetTags;


    void OnValidate()
    {
        if (_collider == null)
            _collider = GetComponent<CircleCollider2D>();
        boomSprite.transform.localScale = new Vector3(damageRadius * 2, damageRadius * 2, 0f);
        _collider.radius = damageRadius;
    }

    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = damageRadius;
        Destroy(gameObject, destructionTime);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetTargetTags(List<string> targetTags)
    {
        _targetTags = targetTags;
    }


    /**
     * <summary>
     * Метод для расчета силы откидывания
     * </summary>
     * <param name="range">Расстояние до цели</param>
     * <param name="maxRange">Максимальное расстояние, на котором влияет взрыв стрелы</param>
     * <returns>Сила откидывания</returns>
     * **/
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
        _collider.enabled = false;
    }

    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, damageRadius);
    }
}
