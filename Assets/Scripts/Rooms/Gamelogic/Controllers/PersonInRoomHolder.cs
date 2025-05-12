using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Компонент, удерживающий и корректирующий положение персонажей внутри зоны комнаты.
/// Требует наличие компонента <see cref="Collider2D"/> для определения границ комнаты.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PersonInRoomHolder : MonoBehaviour
{
    /// <summary>
    /// Список персонажей, находящихся внутри комнаты.
    /// </summary>
    List<Person> _persons;

    /// <summary>
    /// Коллайдер, определяющий границы комнаты.
    /// </summary>
    Collider2D _collider;

    /// <summary>
    /// Инициализация компонентов и списка персонажей.
    /// </summary>
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _persons = new();
    }

    /// <summary>
    /// Добавляет персонажа в список отслеживаемых.
    /// </summary>
    /// <param name="person">Персонаж для добавления.</param>
    public void AddPerson(Person person)
    {
        _persons.Add(person);
    }

    /// <summary>
    /// Корректирует позицию персонажа, чтобы он оставался внутри границ комнаты.
    /// Удаляет персонажа из списка, если он мёртв.
    /// </summary>
    /// <param name="person">Персонаж, позицию которого нужно исправить.</param>
    void FixPersonIn(Person person)
    {
        if (!person.isAlive()) _persons.Remove(person);
        Bounds containerBounds = _collider.bounds;
        Bounds targetBounds = person.GetComponent<Collider2D>().bounds;

        Vector3 dt = Vector3.right + Vector3.up * 2;
        containerBounds.min -= dt - Vector3.up;
        containerBounds.max += dt;

        if (containerBounds.Contains(targetBounds.min) &&
               containerBounds.Contains(targetBounds.max)) return;

        var minBound = containerBounds.min - targetBounds.min;
        var maxBound = containerBounds.max - targetBounds.max;

        dt = Vector3.zero;
        for (int i = 0; i < 3; ++i)
        {
            if (minBound[i] > 0)
                dt[i] += minBound[i] + 1f;
            if (maxBound[i] < 0)
                dt[i] += maxBound[i] - 1f;
        }
        person.transform.position += dt;
    }

    /// <summary>
    /// Вызывается с фиксированным шагом физики.
    /// Корректирует позиции всех отслеживаемых персонажей.
    /// </summary>
    void FixedUpdate()
    {
        if (_persons.Count > 0)
        {
            var persons = new List<Person>(_persons);
            persons.ForEach(p => FixPersonIn(p));
        }
    }
}
