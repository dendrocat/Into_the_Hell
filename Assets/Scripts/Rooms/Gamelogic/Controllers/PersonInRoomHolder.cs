using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PersonInRoomHolder : MonoBehaviour
{
    List<Person> _persons;

    Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _persons = new();
    }

    public void AddPerson(Person person)
    {
        _persons.Add(person);
    }

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

    void FixedUpdate()
    {
        if (_persons.Count > 0)
        {
            var persons = new List<Person>(_persons);
            persons.ForEach(p => FixPersonIn(p));
        }
    }
}
