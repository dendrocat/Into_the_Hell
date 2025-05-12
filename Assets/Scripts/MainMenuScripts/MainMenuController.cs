using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private int _resolutionIndex;

    [Header("Confirmation")]
    [SerializeField] private Image confirmationPromt = null;

    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Deopdowns")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private List<Resolution> uniqueResolutions = new List<Resolution>();

    InputActionAsset _inputActions;

    void Awake()
    {
        _inputActions = InputManager.Instance.GetActions();
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> addedRes = new HashSet<string>();

        _resolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resString = $"{resolutions[i].width} x {resolutions[i].height}";

            if (!addedRes.Contains(resString))
            {
                addedRes.Add(resString);
                options.Add(resString);
                uniqueResolutions.Add(resolutions[i]); // ��������� �����
            }

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                _resolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = _resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        Load();
    }

    void Load()
    {
        SettingsManager.Instance.Load();

        var volume = Convert.ToSingle(SettingsManager.Instance.GetSetting(SettingsKeys.Volume));
        SetVolume(volume);
        volumeSlider.value = volume;

        var brightness = Convert.ToSingle(SettingsManager.Instance.GetSetting(SettingsKeys.Brightness));
        SetBrightness(brightness);
        brightnessSlider.value = _brightnessLevel;

        var fullscreen = Convert.ToBoolean(SettingsManager.Instance.GetSetting(SettingsKeys.FullScreen));
        SetFullScreen(fullscreen);
        fullscreenToggle.isOn = _isFullScreen;
        Screen.fullScreen = _isFullScreen;

        var qualityLevel = Convert.ToInt32(SettingsManager.Instance.GetSetting(SettingsKeys.Quality));
        SetQuality(qualityLevel);
        qualityDropdown.value = _qualityLevel;
        QualitySettings.SetQualityLevel(_qualityLevel);

        var resolutionIndex = Convert.ToInt32(SettingsManager.Instance.GetSetting(SettingsKeys.Resolution));
        SetResolution(resolutionIndex);
        resolutionDropdown.value = _resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        var res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, _isFullScreen);

        var rebinds = Convert.ToString(SettingsManager.Instance.GetSetting(SettingsKeys.Rebinds));
        if (!string.IsNullOrEmpty(rebinds))
            _inputActions.LoadBindingOverridesFromJson(rebinds);
    }

    public void SetResolution(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
    }

    public void TutorialDialogNo()
    {
        GameManager.Instance.NewGame(tutorial: false);
    }

    public void TutorialDialogYes()
    {
        GameManager.Instance.NewGame(tutorial: true);
    }

    public void ContinueGame()
    {
        if (!SaveLoadManager.HasSave())
            noSavedGameDialog.SetActive(true);
        else
            GameManager.Instance.LoadGame();
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    public void SetVolume(float volume)
    {
        CameraSettingsController.Instance.Volume = volume;
        volumeTextValue.text = (volume * 100).ToString("0") + " %";
    }

    public void VolumeApply()
    {
        SettingsManager.Instance.SetSetting(
            SettingsKeys.Volume,
            CameraSettingsController.Instance.Volume
        );

        StartCoroutine(ConfirmationBox());
    }

    public void ControlsApply()
    {
        SettingsManager.Instance.SetSetting(
            SettingsKeys.Rebinds,
            _inputActions.SaveBindingOverridesAsJson()
        );
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.00");
        if (CameraSettingsController.Instance)
            CameraSettingsController.Instance.Brightness = brightness;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        SettingsManager.Instance.SetSetting(SettingsKeys.Brightness, _brightnessLevel);
        SettingsManager.Instance.SetSetting(SettingsKeys.Quality, _qualityLevel);
        SettingsManager.Instance.SetSetting(SettingsKeys.FullScreen, _isFullScreen);
        SettingsManager.Instance.SetSetting(SettingsKeys.Resolution, _resolutionIndex);

        var res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, _isFullScreen);
        QualitySettings.SetQualityLevel(_qualityLevel);

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            _brightnessLevel = (float)SettingsManager.Instance.GetDefaultSetting(SettingsKeys.Brightness);
            brightnessSlider.value = _brightnessLevel;
            brightnessTextValue.text = _brightnessLevel.ToString("0.0");

            _qualityLevel = (int)SettingsManager.Instance.GetDefaultSetting(SettingsKeys.Quality);
            qualityDropdown.value = _qualityLevel;

            _isFullScreen = (bool)SettingsManager.Instance.GetDefaultSetting(SettingsKeys.FullScreen);
            fullscreenToggle.isOn = _isFullScreen;

            _resolutionIndex = (int)SettingsManager.Instance.GetDefaultSetting(SettingsKeys.Resolution);
            resolutionDropdown.value = _resolutionIndex;
            GraphicsApply();
        }

        if (MenuType == "Audio")
        {
            AudioListener.volume = (float)SettingsManager.Instance.GetDefaultSetting(SettingsKeys.Volume);
            volumeSlider.value = AudioListener.volume;
            volumeTextValue.text = AudioListener.volume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Controls")
        {
            _inputActions.RemoveAllBindingOverrides();
            ControlsApply();
        }
    }

    IEnumerator ConfirmationBox()
    {
        SettingsManager.Instance.Save();

        float duration = 1f;
        float delay = 0.01f;
        float t = 0f;
        confirmationPromt.fillAmount = 0;
        confirmationPromt.gameObject.SetActive(true);

        while (t < 1f)
        {
            t += delay / duration;
            confirmationPromt.fillAmount = t;
            yield return new WaitForSecondsRealtime(delay);
        }

        confirmationPromt.gameObject.SetActive(false);
    }
}
