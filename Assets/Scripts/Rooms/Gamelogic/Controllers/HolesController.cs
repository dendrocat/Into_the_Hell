using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер для управления ямами.
/// Обрабатывает взаимодействия с сущностями, которые падают в яму.
/// </summary>
public class HolesController : MonoBehaviour
{
    /// <summary>
    /// Устанавливает тег объекта как "Hole" при инициализации.
    /// </summary>
    void Awake()
    {
        tag = "Hole";
    }

    /// <summary>
    /// Обрабатывает событие входа коллайдера в триггер ямы.
    /// Если объект - персонаж без эффектов Shift и HoleStun, добавляет эффект оглушения и наносит урон.
    /// Запускает корутину обработки выхода из ямы.
    /// </summary>
    /// <param name="collision">Коллайдер объекта, вошедшего в яму.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        Person person = collision.gameObject.GetComponent<Person>();
        if (person)
        {
            if (!person.hasEffect(EffectNames.Shift) && !person.hasEffect(EffectNames.HoleStun))
            {
                person.AddEffect(EffectNames.HoleStun);
                person.TakeDamage(25f, DamageType.None);

                //personAnimator.SetTrigger("fallTrigger");
                Vector2 direction = person.transform.position - transform.position;
                direction = direction.normalized;
                direction = ((direction != Vector2.zero) ? direction : Vector2.right) * 1.5f;

                StartCoroutine(HoleExitHandler(person, direction));
            }
        }
    }

    /// <summary>
    /// Внутренний метод, использующийся в корутине обработки падения в яму.
    /// </summary>
    /// <param name="person">Упавшая сущность.</param>
    /// <returns><see langword="true"/>, если у сущности есть эффект <see cref="EffectNames.HoleStun"/>; иначе <see langword="false"/>.</returns>
    bool FallingPersonHasEffect(Person person)
    {
        return person.hasEffect(EffectNames.HoleStun);
    }

    /// <summary>
    /// Корутина, обрабатывающая падение в яму.
    /// </summary>
    /// <param name="fallingPerson">Падающая сущность</param>
    /// <param name="direction">Точка, в которой сущность появится после падения</param>
    /// <returns>IEnumerator для корутины.</returns>
    IEnumerator HoleExitHandler(Person fallingPerson, Vector2 direction)
    {
        yield return new WaitWhile(() => FallingPersonHasEffect(fallingPerson));
        fallingPerson.transform.position = transform.position + (Vector3)direction;
    }
}
