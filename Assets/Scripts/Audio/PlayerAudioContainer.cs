using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAudioName
{
    Walk, Dash, Heal,
    Damage, DamageShield,
    Coins, Die,
    Sword, Shield, TwoHandedSword, Bow
}


[CreateAssetMenu(fileName = "PlayerAudioContainer", menuName = "Audio Objects/PlayerAudioContainer")]
public class PlayerAudioContainer : ScriptableObject
{
    [Serializable]
    public struct NameClip
    {
        public PlayerAudioName name;
        public AudioClip clip;
    }
    [SerializeField] List<NameClip> _nameClips;

    public List<NameClip> Clips => _nameClips;
}
