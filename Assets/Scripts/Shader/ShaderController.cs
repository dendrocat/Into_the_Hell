using UnityEngine;

/// <summary>
/// Контроллер шейдера камеры, управляющий яркостью изображения.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ShaderController : MonoBehaviour
{
    /// <summary>
    /// Материал с шейдером, который применяется к камере.
    /// </summary>
    [Tooltip("Материал с шейдером для обработки изображения")]
    public Material shader;

    [SerializeField] private float _brightness = 0.5f;

    /// <summary>
    /// Яркость изображения, применяется к шейдеру.
    /// Значение масштабируется на 0.5.
    /// </summary>
    public float Brightness
    {
        get => _brightness;
        set
        {
            _brightness = 0.5f * value;
        }
    }

    /// <summary>
    /// Вызывается после рендера камеры для применения шейдера.
    /// </summary>
    /// <param name="src">Исходный RenderTexture.</param>
    /// <param name="dest">Целевой RenderTexture.</param>
    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        shader.SetFloat("_Brightness", _brightness);
        Graphics.Blit(src, dest, shader);
    }
}
