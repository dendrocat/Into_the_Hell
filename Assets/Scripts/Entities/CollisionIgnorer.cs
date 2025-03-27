using UnityEngine;
using System.Collections.Generic;

public class CollisionIgnorer : MonoBehaviour
{
    public List<string> ignoredTags = new List<string>();
    private HashSet<Collider2D> ignoredColliders = new HashSet<Collider2D>();

    void Start()
    {
        // Игнорирование коллизий при старте для существующих объектов
        foreach (var tag in ignoredTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (var obj in objects)
            {
                IgnoreCollisionsWithObject(obj);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Динамическое игнорирование для новых объектов
        if (ignoredTags.Contains(collision.gameObject.tag))
        {
            IgnoreCollisionsWithObject(collision.gameObject);
        }
    }

    private void IgnoreCollisionsWithObject(GameObject target)
    {
        Collider2D[] ourColliders = GetComponents<Collider2D>();
        Collider2D[] theirColliders = target.GetComponents<Collider2D>();

        foreach (var ourCollider in ourColliders)
        {
            foreach (var theirCollider in theirColliders)
            {
                if (!ignoredColliders.Contains(theirCollider))
                {
                    Physics2D.IgnoreCollision(ourCollider, theirCollider);
                    ignoredColliders.Add(theirCollider);
                }
            }
        }
    }
}
