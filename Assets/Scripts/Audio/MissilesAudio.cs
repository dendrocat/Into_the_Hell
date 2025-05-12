using UnityEngine;

/// <summary>
/// Компонент для воспроизведения звуков запуска и попадания снаряда.
/// </summary>
public class MissilesAudio : MonoBehaviour
{
    /// <summary>
    /// Звук запуска снаряда (необязательный).
    /// </summary>
    [SerializeField] Optional<AudioClip> _missileStart;

    /// <summary>
    /// Звук попадания снаряда (необязательный).
    /// </summary>
    [SerializeField] Optional<AudioClip> _missileHit;

    /// <summary>
    /// Метод Unity, вызываемый при инициализации объекта.
    /// Воспроизводит звук запуска снаряда, если он есть.
    /// </summary>
    void Awake()
    {
        if (_missileStart.Enabled)
            PlaySound(_missileStart.Value);
    }

    /// <summary>
    /// Воспроизводит переданный аудиоклип в позиции текущего объекта.
    /// Создаёт временный GameObject с AudioSource, который уничтожается по окончании воспроизведения.
    /// </summary>
    /// <param name="clip">Аудиоклип для воспроизведения.</param>
    void PlaySound(AudioClip clip)
    {
        var obj = new GameObject($"Sound_{clip.name}");
        obj.transform.position = transform.position;

        var audio = obj.AddComponent<AudioSource>();
        audio.volume = .25f;
        audio.clip = clip;
        audio.loop = false;
        audio.Play();

        Destroy(obj, clip.length);
    }

    /// <summary>
    /// Метод Unity, вызываемый при уничтожении объекта.
    /// Воспроизводит звук попадания снаряда, если он есть.
    /// </summary>
    void OnDestroy()
    {
        if (_missileHit.Enabled)
            PlaySound(_missileHit.Value);
    }
}
