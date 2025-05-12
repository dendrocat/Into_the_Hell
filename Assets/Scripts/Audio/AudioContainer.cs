using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скриптовый объект, предназначенный для хранения коллекции аудиоклипов с их именами.
/// </summary>
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
        [Tooltip("Имя аудиоклипа для идентификации")]
        public string name;

        /// <summary>
        /// Аудио.
        /// </summary>
        [Tooltip("Аудиоклип, связанный с этим именем")]
        public AudioClip clip;
    }

    /// <summary>
    /// Список пар "имя - аудио", хранящихся в контейнере.
    /// </summary>
    [Tooltip("Список аудиоклипов с их уникальными именами")]
    [SerializeField] List<NameClip> _nameClips;

    /// <summary>
    /// Публичное свойство для доступа к списку аудиоклипов.
    /// </summary>
    public List<NameClip> Clips => _nameClips;
}
