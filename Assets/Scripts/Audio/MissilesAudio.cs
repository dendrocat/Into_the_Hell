using UnityEngine;

public class MissilesAudio : MonoBehaviour
{
    [SerializeField] AudioClip _missileStart;
    [SerializeField] Optional<AudioClip> _missileHit;

    void Awake()
    {
        PlaySound(_missileStart);
    }

    void PlaySound(AudioClip clip)
    {
        var obj = new GameObject($"Sound_{clip.name}");
        obj.transform.position = transform.position;

        var audio = obj.AddComponent<AudioSource>();
        audio.volume = 0.05f;
        audio.clip = clip;
        audio.loop = false;
        audio.Play();

        Destroy(obj, clip.length);
    }

    public void PlayHitSound()
    {
        if (!_missileHit.Enabled) return;
        PlaySound(_missileHit.Value);
    }
}
