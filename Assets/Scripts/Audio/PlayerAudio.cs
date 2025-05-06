using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInventory), typeof(Player))]
public class PlayerAudio : AudioPlayer
{
    [SerializeField] PlayerAudioContainer audioContainer;

    Dictionary<PlayerAudioName, AudioClip> _clips;

    Player _player;

    PlayerAudioName currentAudio;

    void Start()
    {
        _clips = new();
        foreach (var item in audioContainer.Clips)
        {
            _clips[item.name] = item.clip;
        }
        var inventory = GetComponent<PlayerInventory>();
        inventory.MoneyChanged.AddListener(
            (money) => Play(PlayerAudioName.Coins)
        );
        _player = GetComponent<Player>();
    }

    public void Play(PlayerAudioName audioName)
    {
        if (audioName == PlayerAudioName.Walk)
            Loop = true;
        else Loop = false;
        Play(_clips.GetValueOrDefault(audioName, null));

        currentAudio = audioName;
    }

    void Update()
    {
        if (_player.isMoving())
        {
            if (currentClip == null)
                Play(PlayerAudioName.Walk);
        }
        else if (currentAudio == PlayerAudioName.Walk)
            Stop();
    }
}
