using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс, который отвечает за нанесение урона объектам между связанными огненными сферами.
/// </summary>
public class FireSphereAttack : MonoBehaviour
{
    /// <summary>
    /// Ссылка на компонент <see cref="FireSphere"/>, к которому привязан этот скрипт.
    /// </summary>
    FireSphere sphere;

    /// <summary>
    /// Периодичность нанесения урона (в секундах).
    /// </summary>
    [Tooltip("Периодичность нанесения урона (в секундах)")]
    [SerializeField] float period = 0.25f;

    /// <summary>
    /// Инициализация компонента и запуск корутины атаки.
    /// </summary>
    private void Start()
    {
        sphere = GetComponent<FireSphere>();
        period = 0.25f;
        StartCoroutine(FireLineAttack());
    }

    /// <summary>
    /// Корутина, которая периодически наносит урон объектам, находящимся между связанными сферами.
    /// </summary>
    /// <returns><see cref="IEnumerator"/> для запуска корутины.</returns>
    IEnumerator FireLineAttack()
    {
        yield return new WaitForSeconds(period);

        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, sphere.linkedSphere.transform.position);

        HashSet<GameObject> intersectedObjects = new HashSet<GameObject>();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject &&
                hit.collider.gameObject != sphere.linkedSphere.gameObject) // не сферы
            {
                List<string> targetTags = sphere.GetTargetTags();
                foreach (string targetTag in targetTags)
                {
                    if (hit.collider.gameObject.CompareTag(targetTag))
                    {
                        intersectedObjects.Add(hit.collider.gameObject);
                        break;
                    }
                }
            }
        }

        foreach (GameObject candidate in intersectedObjects)
        {
            IDamagable damagable;
            if (candidate.TryGetComponent<IDamagable>(out damagable))
            {
                damagable.TakeDamage(sphere.GetDamage(), DamageType.Fire);
            }
        }

        StartCoroutine(FireLineAttack());
    }
}
