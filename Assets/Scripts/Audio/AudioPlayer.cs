using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioPlayer : MonoBehaviour
{
    AudioSource _source;

    protected float Volume => _source.volume;

    protected abstract void InitAudio();

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        InitAudio();
    }

    protected void Play(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
        StartCoroutine(WaitEndClip());
    }

    IEnumerator WaitEndClip()
    {
        yield return new WaitUntil(() => !_source.isPlaying);
        _source.clip = null;
    }

}
