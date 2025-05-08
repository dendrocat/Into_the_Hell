using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShaderController : MonoBehaviour
{
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

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        shader.SetFloat("_Brightness", _brightness);
        Graphics.Blit(src, dest, shader);
    }
}
