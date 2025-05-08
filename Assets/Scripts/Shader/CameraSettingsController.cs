using UnityEngine;

[RequireComponent(typeof(ShaderController))]
public class CameraSettingsController : MonoBehaviour
{
    public static CameraSettingsController Instance { get; private set; } = null;

    ShaderController _shader;

    public float Brightness { get => _shader.Brightness; set => _shader.Brightness = value; }

    [SerializeField]
    [Range(0f, 1f)] float volume;
    public float Volume
    {
        get => AudioListener.volume;
        set
        {
            volume = value;
            AudioListener.volume = value;
        }
    }

    void Awake()
    {
        Instance = this;
        _shader = GetComponent<ShaderController>();
    }

    void Start()
    {
        Brightness = (float)SettingsManager.Instance.GetSetting(SettingsKeys.Brightness);
        Volume = (float)SettingsManager.Instance.GetSetting(SettingsKeys.Volume);
    }

    // //TODO: remove this after debug
    // void Update()
    // {
    //     Volume = volume;
    // }
}
