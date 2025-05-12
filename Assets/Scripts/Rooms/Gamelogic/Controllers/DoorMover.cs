using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент, управляющий движением двери с анимацией и звуковым сопровождением.
/// Требует наличие компонентов <see cref="AudioSource"/> и <see cref="BoxCollider2D"/>.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DoorMover : MonoBehaviour
{
    /// <summary>
    /// Смещение двери при открытии/закрытии.
    /// </summary>
    [Tooltip("Смещение двери при открытии/закрытии")]
    [SerializeField] Vector2 delta;

    /// <summary>
    /// Длительность движения двери (в секундах).
    /// </summary>
    [Tooltip("Длительность движения двери (в секундах)")]   
    [SerializeField, Range(0.1f, 1f)] float moveDuration;

    /// <summary>
    /// Длительность звукового эффекта (в секундах).
    /// </summary>
    [Tooltip("Длительность звукового эффекта (в секундах)")]
    [SerializeField, Range(0.1f, 1f)] float audioDuration;

    AudioSource _audio;

    BoxCollider2D _collider;

    /// <summary>
    /// Инициализация компонентов и настройка параметров аудио.
    /// </summary>
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.pitch = _audio.clip.length / audioDuration;

        _collider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Воспроизводит звуковой эффект открытия/закрытия двери.
    /// </summary>
    void Play()
    {
        _audio.Play();
    }

    /// <summary>
    /// Запускает анимацию открытия двери.
    /// </summary>
    public void OpenDoor()
    {
        StartCoroutine(MoveDoor(-1));
    }

    /// <summary>
    /// Запускает анимацию закрытия двери.
    /// </summary>
    public void CloseDoor()
    {
        StartCoroutine(MoveDoor(1));
    }

    /// <summary>
    /// Обновляет размер коллайдера двери в зависимости от направления движения.
    /// </summary>
    /// <param name="modifier">Множитель для изменения размера коллайдера.</param>
    void UpdateCollider(float modifier)
    {
        var size = _collider.size;
        if (size.x > size.y) size.x *= modifier;
        else size.y *= modifier;
        _collider.size = size;
    }

    /// <summary>
    /// Корутина для плавного перемещения двери в заданном направлении.
    /// </summary>
    /// <param name="direction">Направление движения: 
    /// <list type="bullet">
    /// <item><description>1 - закрытие</description></item>
    /// <item><description>-1 - открытие</description></item>
    /// </list>
    /// </param>
    /// <returns>IEnumerator для корутины.</returns>
    IEnumerator MoveDoor(sbyte direction)
    {
        Play();
        UpdateCollider(direction == 1 ? 3 : 1 / 3);
        float t = 0;
        Vector3 start = transform.position;
        Vector3 end = transform.position + (Vector3)delta * direction;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, end, t);
            t += Time.deltaTime / moveDuration;
            yield return null;
        }
        transform.position = end;
        if (direction == -1)
            Destroy(gameObject, audioDuration - moveDuration);
    }
}
