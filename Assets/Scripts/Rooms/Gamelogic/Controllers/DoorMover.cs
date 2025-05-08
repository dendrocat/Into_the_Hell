using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DoorMover : MonoBehaviour
{
    [SerializeField] Vector2 delta;
    [SerializeField]
    [Range(0.1f, 1f)] float moveDuration;

    [SerializeField]
    [Range(0.1f, 1f)] float audioDuration;

    AudioSource _audio;

    BoxCollider2D _collider;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.pitch = _audio.clip.length / audioDuration;

        _collider = GetComponent<BoxCollider2D>();
    }

    void Play()
    {
        _audio.Play();
    }

    public void OpenDoor()
    {
        StartCoroutine(MoveDoor(-1));
    }

    public void CloseDoor()
    {
        StartCoroutine(MoveDoor(1));
    }

    void UpdateCollider(float modifier)
    {
        var size = _collider.size;
        if (size.x > size.y) size.x *= modifier;
        else size.y *= modifier;
        _collider.size = size;
    }

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
