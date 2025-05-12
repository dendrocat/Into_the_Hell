using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скриптовый объект, предназначенный для хранения коллекции аудиоклипов с их именами.
/// </summary>
/// <remarks>
/// Позволяет удобно организовывать и использовать наборы аудиоклипов в проекте Unity.
/// Используется в качестве контейнера для пар "имя - аудио".
/// </remarks>
[CreateAssetMenu(fileName = "AudioContainer", menuName = "Audio Objects/Audio Container")]
public class AudioContainer : ScriptableObject
{
    // <summary>
    /// Структура, представляющая пару "имя - аудио".
    /// </summary>
    [Serializable]
    public struct NameClip
    {
        /// <summary>
        /// Имя аудио.
        /// </summary>
        public string name;

        /// <summary>
        /// Аудио.
        /// </summary>
        public AudioClip clip;
    }

    /// <summary>
    /// Список пар "имя - аудио", хранящихся в контейнере.
    /// </summary>
    [SerializeField] List<NameClip> _nameClips;

    /// <summary>
    /// Публичное свойство для доступа к списку аудиоклипов.
    /// </summary>
    public List<NameClip> Clips => _nameClips;
}
