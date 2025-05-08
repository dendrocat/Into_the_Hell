using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShaderController : MonoBehaviour
{
    public static ShaderController Instance { get; private set; }
    public Material shader;

    [SerializeField]
    private float _brightness = 0.5f;
    public float Brightness
    {
        get => _brightness;
        set
        {
            _brightness = 0.5f * value;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        shader.SetFloat("_Brightness", _brightness);
        Graphics.Blit(src, dest, shader);
    }

    public void Start()
    {
        _brightness = (float)SettingsManager.Instance.GetSetting(SettingsKeys.Brightness) * 0.5f;
    }
}
