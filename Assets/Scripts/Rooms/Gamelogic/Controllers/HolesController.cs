using System.Collections;
using UnityEngine;

/// <summary>
/// Controller for managing holes in the tilemap.
/// Handles interactions with entities that fall into holes.
/// </summary>
public class HolesController : MonoBehaviour
{
    /**
     * <summary>
     * Метод, обрабатывающий вход сущности в яму.
     * </summary>
     * <param name="collision">Коллайдер сущности.</param>
     * **/
    void OnTriggerEnter2D(Collider2D collision)
    {
        Person person = collision.GetComponent<Person>();
        if (person)
        {
            if (!person.hasEffect(EffectNames.Shift) && !person.hasEffect(EffectNames.HoleStun))
            {
                person.TakeDamage(25f, DamageType.None);
                person.AddEffect(EffectNames.HoleStun);

                //personAnimator.SetTrigger("fallTrigger");

                Vector2 direction = person.transform.position - transform.position;
                direction = direction.normalized;
                direction = ((direction != Vector2.zero) ? direction : Vector2.right) * 1.5f;

                StartCoroutine(HoleExitHandler(person, direction));
            }
        }
    }

    /**
     * <summary>
     * Внутренний метод, использующийся в корутине обработки падения в яму.
     * </summary>
     * <param name="person">Упавшая сущность.</param>
     * **/
    bool FallingPersonHasEffect(Person person)
    {
        return person.hasEffect(EffectNames.HoleStun);
    }

    /**
     * <summary>
     * Корутина, обрабатывающая падение в яму.
     * </summary>
     * <param name="direction">Точка, в которой сущность появится после падения</param>
     * <param name="fallingPerson">Падающая сущность</param>
     * **/
    IEnumerator HoleExitHandler(Person fallingPerson, Vector2 direction)
    {
        yield return new WaitWhile(() => FallingPersonHasEffect(fallingPerson));
        fallingPerson.transform.position = transform.position + (Vector3) direction;
    }
}
