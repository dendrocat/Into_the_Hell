using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioPlayer : MonoBehaviour
{
    AudioSource _source;

    public AudioClip currentClip => _source.clip;

    protected bool Loop { get => _source.loop; set => _source.loop = value; }

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    protected void Play(AudioClip clip)
    {
        if (clip != currentClip)
            _source.clip = clip;
        _source.Play();
        if (!Loop) StartCoroutine(WaitEndClip());
    }

    IEnumerator WaitEndClip()
    {
        yield return new WaitUntil(() => !_source.isPlaying);
        Stop();
    }

    protected void Stop()
    {
        _source.clip = null;
    }
}
