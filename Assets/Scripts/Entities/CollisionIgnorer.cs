using UnityEngine;
using System.Collections.Generic;

/**
 * <summary>
 * Класс, обрабатывающий игнорирование коллизий с объектами, имеющими некоторые теги.
 * </summary>
 * **/
public class CollisionIgnorer : MonoBehaviour
{
    [SerializeField] List<string> ignoredTags = new List<string>();
    private HashSet<Collider2D> ignoredColliders = new HashSet<Collider2D>();

    /**
     * <summary>
     * Добавление имеющихся на старте объектов в список игнорируемых
     * </summary>
     * **/
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

    /**
     * <summary>
     * При столкновении с объектом проверяет его тег: если он есть
     * в списке игнориуемых тегов, то добавляет объект в список игнорируемых.
     * </summary>
     * **/
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Динамическое игнорирование для новых объектов
        if (ignoredTags.Contains(collision.gameObject.tag))
        {
            IgnoreCollisionsWithObject(collision.gameObject);
        }
    }

    /**
     * <summary>
     * Метод, обрабатывающий игнорирование коллизии с объектом
     * </summary>
     * **/
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
