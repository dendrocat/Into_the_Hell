using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShaderController : MonoBehaviour {
    public Material shader;
    public float Brightness = 0.5f;

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        shader.SetFloat("_Brightness", Brightness);
        Graphics.Blit(src, dest, shader);
    }

    public void Start() {
        Brightness = (float)SettingsManager.Instance.GetSetting(SettingsKeys.Brightness)*0.5f;
    }
}
