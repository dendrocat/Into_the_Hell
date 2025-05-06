using UnityEngine;

public class MissilesAudio : MonoBehaviour
{
    [SerializeField] Optional<AudioClip> _missileStart;
    [SerializeField] Optional<AudioClip> _missileHit;

    void Awake()
    {
        if (_missileStart.Enabled)
            PlaySound(_missileStart.Value);
    }

    void PlaySound(AudioClip clip)
    {
        var obj = new GameObject($"Sound_{clip.name}");
        obj.transform.position = transform.position;

        var audio = obj.AddComponent<AudioSource>();
        audio.volume = 0.07f;
        audio.clip = clip;
        audio.loop = false;
        audio.Play();

        Destroy(obj, clip.length);
    }

    public void PlayHitSound()
    {
        if (_missileHit.Enabled)
            PlaySound(_missileHit.Value);
    }
}
