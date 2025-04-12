using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Класс, который отвечает за нанесение урона объектам между связанными огненными сферами.
 * </summary>
 * **/
public class FireSphereAttack : MonoBehaviour
{
    FireSphere sphere;
    float period;

    private void Start()
    {
        sphere = GetComponent<FireSphere>();
        period = 0.25f;
        StartCoroutine(FireLineAttack());
    }

    IEnumerator FireLineAttack()
    {
        yield return new WaitForSeconds(0.25f);

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
