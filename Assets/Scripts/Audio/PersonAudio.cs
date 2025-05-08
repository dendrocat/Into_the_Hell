using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Person))]
public class PersonAudio : AudioPlayer
{
    public const string WALK = "Walk";
    [SerializeField] AudioContainer audioContainer;

    Dictionary<string, AudioClip> _clips;

    Person _person;

    AudioSource _walk;

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

    public void Play(string audioName)
    {
        Play(_clips.GetValueOrDefault(audioName, null));
    }

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
