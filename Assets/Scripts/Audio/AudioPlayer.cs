using System.Collections;
using UnityEngine;

/// <summary>
/// Абстрактный класс для воспроизведения аудиоклипов с использованием компонента AudioSource.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// Ссылка на компонент AudioSource, используемый для воспроизведения звука.
    /// </summary>
    AudioSource _source;

    /// <summary>
    /// Текущая громкость воспроизведения аудио.
    /// </summary>
    protected float Volume => _source.volume;

     /// <summary>
    /// Длина текущего воспроизводимого аудиоклипа в секундах.
    /// </summary>
    public float CurrentClipLength => _source.clip.length;

    /// <summary>
    /// Инициализация аудиосистемы персонажа.
    /// </summary>
    protected abstract void InitAudio();

    /// <summary>
    /// Метод Unity, вызываемый при инициализации объекта.
    /// Получает компонент AudioSource и вызывает инициализацию аудио.
    /// </summary>
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        InitAudio();
    }

    /// <summary>
    /// Запускает воспроизведение переданного аудиоклипа.
    /// </summary>
    /// <param name="clip">Аудиоклип для воспроизведения.</param>
    protected void Play(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
        StartCoroutine(WaitEndClip());
    }

    /// <summary>
    /// Корутина, ожидающая окончания воспроизведения аудиоклипа.
    /// После завершения воспроизведения очищает текущий клип.
    /// </summary>
    IEnumerator WaitEndClip()
    {
        yield return new WaitUntil(() => !_source.isPlaying);
        _source.clip = null;
    }

}
