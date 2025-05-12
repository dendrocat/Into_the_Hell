using UnityEngine;

/// <summary>
/// Контроллер настроек камеры, управляющий яркостью и уровнем звука.
/// </summary>
[RequireComponent(typeof(ShaderController))]
public class CameraSettingsController : MonoBehaviour
{
    /// <summary>
    /// Singleton-экземпляр контроллера.
    /// </summary>
    public static CameraSettingsController Instance { get; private set; } = null;

    ShaderController _shader;

    /// <summary>
    /// Яркость, управляемая через ShaderController.
    /// </summary>
    public float Brightness { get => _shader.Brightness; set => _shader.Brightness = value; }

    /// <summary>
    /// Громкость звука, синхронизированная с AudioListener.
    /// </summary>
    public float Volume
    {
        get => AudioListener.volume;
        set
        {
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
}
