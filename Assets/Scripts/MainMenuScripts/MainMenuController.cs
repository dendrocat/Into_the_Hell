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
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1.0f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private int _resolutionIndex;

    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPromt = null;

    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Deopdowns")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private List<Resolution> uniqueResolutions = new List<Resolution>();

    private SettingsManager _settingsManager;

    InputActionAsset _inputActions;

    void Awake()
    {
        InputManager.Instance.PushInputMap(InputMap.UI);
        _inputActions = InputManager.Instance.GetActions();
        _settingsManager = new SettingsManager();
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
        var prefs = _settingsManager.Load();

        var volume = Convert.ToSingle(prefs.GetValueOrDefault(SettingsKeys.Volume, defaultVolume));
        SetVolume(volume);
        volumeSlider.value = AudioListener.volume;

        var brightness = Convert.ToSingle(prefs.GetValueOrDefault(SettingsKeys.Brightness, defaultBrightness));
        SetBrightness(brightness);
        brightnessSlider.value = _brightnessLevel;

        var fullscreen = Convert.ToBoolean(prefs.GetValueOrDefault(SettingsKeys.FullScreen, true));
        SetFullScreen(fullscreen);
        fullscreenToggle.isOn = _isFullScreen;
        Screen.fullScreen = _isFullScreen;

        var qualityLevel = Convert.ToInt32(prefs.GetValueOrDefault(SettingsKeys.Quality, 0));
        SetQuality(qualityLevel);
        qualityDropdown.value = _qualityLevel;
        QualitySettings.SetQualityLevel(_qualityLevel);

        var resolutionIndex = Convert.ToInt32(prefs.GetValueOrDefault(SettingsKeys.Resolution, resolutions.Length - 1));
        SetResolution(resolutionIndex);
        resolutionDropdown.value = _resolutionIndex;
        resolutionDropdown.RefreshShownValue();
        var res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, _isFullScreen);

        var rebinds = Convert.ToString(prefs.GetValueOrDefault(SettingsKeys.Rebinds, ""));
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
        AudioListener.volume = volume;
        volumeTextValue.text = (volume * 100).ToString("0") + " %";
    }

    public void VolumeApply()
    {
        _settingsManager.AddToSave(SettingsKeys.Volume, AudioListener.volume);

        StartCoroutine(ConfirmationBox());
    }

    public void ControlsApply()
    {
        _settingsManager.AddToSave(
            SettingsKeys.Rebinds,
            _inputActions.SaveBindingOverridesAsJson()
        );
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
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
        _settingsManager.AddToSave(SettingsKeys.Brightness, _brightnessLevel);
        _settingsManager.AddToSave(SettingsKeys.Quality, _qualityLevel);
        _settingsManager.AddToSave(SettingsKeys.FullScreen, _isFullScreen);
        _settingsManager.AddToSave(SettingsKeys.Resolution, _resolutionIndex);

        var res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, _isFullScreen);
        QualitySettings.SetQualityLevel(_qualityLevel);

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            _brightnessLevel = defaultBrightness;
            brightnessSlider.value = _brightnessLevel;
            brightnessTextValue.text = _brightnessLevel.ToString("0.0");

            _qualityLevel = 1;
            qualityDropdown.value = _qualityLevel;

            _isFullScreen = false;
            fullscreenToggle.isOn = _isFullScreen;

            _resolutionIndex = resolutions.Length - 1;
            resolutionDropdown.value = _resolutionIndex;
            GraphicsApply();
        }

        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Controls")
        {
            _inputActions.RemoveAllBindingOverrides();
            ControlsApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPromt.SetActive(true);
        _settingsManager.Save();
        yield return new WaitForSeconds(2);
        comfirmationPromt.SetActive(false);
    }
}
