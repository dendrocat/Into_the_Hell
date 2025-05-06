using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioContainer", menuName = "Audio Objects/Audio Container")]
public class AudioContainer : ScriptableObject
{
    [Serializable]
    public struct NameClip
    {
        public string name;
        public AudioClip clip;
    }
    [SerializeField] List<NameClip> _nameClips;

    public List<NameClip> Clips => _nameClips;
}
