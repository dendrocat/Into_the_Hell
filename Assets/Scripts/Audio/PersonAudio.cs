using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для управления аудио объекта класса <see cref="Person"/>, наследующий функциональность <see cref="AudioPlayer"/>.
/// </summary>
[RequireComponent(typeof(Person))]
public class PersonAudio : AudioPlayer
{
    /// <summary>
    /// Ключ для звука шагов в <see cref="AudioContainer"/>.
    /// </summary>
    public const string WALK = "Walk";

    /// <summary>
    /// <see cref="AudioContainer">Контейнер</see> с звуками персонажа.
    /// </summary>
    [Tooltip("Контейнер с звуками персонажа")]
    [SerializeField] AudioContainer audioContainer;

    /// <summary>
    /// Словарь для быстрого доступа к аудиоклипам по именам
    /// </summary>
    Dictionary<string, AudioClip> _clips;

    /// <summary>
    /// Ссылка на компонент <see cref="Person"/>
    /// </summary>
    Person _person;

    /// <summary>
    /// Отдельный источник звука для шагов
    /// </summary>
    AudioSource _walk;

    /// <inheritdoc />
    protected override void InitAudio()
    {
        _clips = new();
        foreach (var item in audioContainer.Clips)
        {
            _clips[item.name] = item.clip;
        }
        if (!_clips.ContainsKey(WALK))
        {
            Debug.LogError($"Missing {WALK} sound");
        }
        _person = GetComponent<Person>();

        var obj = new GameObject("WalkSound");
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;

        _walk = obj.AddComponent<AudioSource>();
        _walk.loop = true;
        _walk.volume = Volume * .5f;
        _walk.clip = _clips[WALK];
        _walk.Pause();
    }

    /// <summary>
    /// Воспроизведение звука по имени из контейнера.
    /// </summary>
    /// <param name="audioName">Имя аудиоклипа.</param>
    public void Play(string audioName)
    {
        Play(_clips.GetValueOrDefault(audioName, null));
    }

    /// <summary>
    /// Обновление состояния звука шагов в зависимости от движения персонажа.
    /// </summary>
    void Update()
    {
        if (_person.isMoving() && _person.isAlive())
        {
            if (!_walk.isPlaying)
                _walk.Play();
        }
        else if (_walk.isPlaying)
            _walk.Stop();
    }
}
