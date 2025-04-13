using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSentextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

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

    [Header("Levels to load")]
    public string _newGameLevel;
    public string _tutorialLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Deopdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private SettingsManager _settingsManager;

    void Awake()
    {
        _settingsManager = new SettingsManager();
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        _resolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

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

        invertYToggle.isOn = Convert.ToBoolean(prefs.GetValueOrDefault(SettingsKeys.InvertY, false));

        var sensitivity = Convert.ToInt32(prefs.GetValueOrDefault(SettingsKeys.Sensitivity, defaultSen));
        SetControllerSen(sensitivity);
        controllerSenSlider.value = mainControllerSen;

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
    }

    public void SetResolution(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void TutorialDialogNo()
    {
        SceneManager.LoadScene(_tutorialLevel);
    }

    public void LoadGameDialogYes()
    {
        if (GameRepository.HasSave())
        {
            //TODO: replace to load level with base
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        _settingsManager.AddToSave(SettingsKeys.Volume, AudioListener.volume);

        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        ControllerSentextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        _settingsManager.AddToSave(SettingsKeys.InvertY, invertYToggle.isOn);
        _settingsManager.AddToSave(SettingsKeys.Sensitivity, mainControllerSen);

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
        Screen.fullScreen = _isFullScreen;

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

        if (MenuType == "Gameplay")
        {
            ControllerSentextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
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
