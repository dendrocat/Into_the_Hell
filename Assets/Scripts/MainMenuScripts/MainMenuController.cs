using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Контроллер главного меню.
/// Отвечает за обработку настроек звука, графики, управления, разрешения, полноэкранного режима,
/// а также за запуск и продолжение игры, отображение диалогов и подтверждений.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Настройки громкости")]

    /// <summary>
    /// Текстовое поле для отображения текущего значения громкости.
    /// </summary>
    [Tooltip("Текстовое поле для отображения текущего значения громкости")]
    [SerializeField] private TMP_Text volumeTextValue = null;

    /// <summary>
    /// Слайдер для регулировки громкости.
    /// </summary>
    [Tooltip("Слайдер для регулировки громкости")]
    [SerializeField] private Slider volumeSlider = null;


    [Header("Настройки графики")]

    /// <summary>
    /// Слайдер для регулировки яркости.
    /// </summary>
    [Tooltip("Слайдер для регулировки яркости")]
    [SerializeField] private Slider brightnessSlider = null;

    // <summary>
    /// Текстовое поле для отображения текущего значения яркости.
    /// </summary>
    [Tooltip("Текстовое поле для отображения текущего значения яркости")]
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Space(10)]
    /// <summary>
    /// Выпадающий список для выбора качества графики.
    /// </summary>
    [Tooltip("Выпадающий список для выбора качества графики")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    /// <summary>
    /// Переключатель полноэкранного режима.
    /// </summary>
    [Tooltip("Переключатель полноэкранного режима")]
    [SerializeField] private Toggle fullscreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;
    private int _resolutionIndex;

    [Header("Подтверждение")]
    /// <summary>
    /// Изображение для отображения анимации подтверждения применения настроек.
    /// </summary>
    [Tooltip("Изображение для подтверждения применения настроек")]
    [SerializeField] private Image confirmationPromt = null;

    /// <summary>
    /// Диалоговое окно при отсутствии сохранённой игры.
    /// </summary>
    [Tooltip("Диалоговое окно при отсутствии сохранённой игры")]
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Разрешения экрана")]
    /// <summary>
    /// Выпадающий список для выбора разрешения экрана.
    /// </summary>
    [Tooltip("Выпадающий список для выбора разрешения экрана")]
    [SerializeField] TMP_Dropdown resolutionDropdown;

    /// <summary>
    /// Все доступные разрешения экрана.
    /// </summary>
    private Resolution[] resolutions;

    /// <summary>
    /// Уникальные разрешения экрана для выпадающего списка.
    /// </summary>
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    /// <summary>
    /// Ссылка на действия ввода пользователя.
    /// </summary>
    InputActionAsset _inputActions;

    /// <summary>
    /// Инициализация ссылок и загрузка настроек ввода.
    /// </summary>
    void Awake()
    {
        _inputActions = InputManager.Instance.GetActions();
    }

    /// <summary>
    /// Инициализация выпадающих списков, загрузка настроек и установка значений по умолчанию.
    /// </summary>
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
                uniqueResolutions.Add(resolutions[i]);
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

    // <summary>
    /// Загрузка и применение сохранённых настроек.
    /// </summary>
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

    /// <summary>
    /// Устанавливает выбранное разрешение экрана (индекс в списке).
    /// </summary>
    /// <param name="resolutionIndex">Индекс разрешения.</param>
    public void SetResolution(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
    }

    /// <summary>
    /// Запускает новую игру без обучения.
    /// </summary>
    public void TutorialDialogNo()
    {
        GameManager.Instance.NewGame(tutorial: false);
    }

    /// <summary>
    /// Запускает новую игру с обучением.
    /// </summary>
    public void TutorialDialogYes()
    {
        GameManager.Instance.NewGame(tutorial: true);
    }

    /// <summary>
    /// Продолжает игру, если есть сохранение. Иначе - показывает диалог об отсутствии сохранения.
    /// </summary>
    public void ContinueGame()
    {
        if (!SaveLoadManager.HasSave())
            noSavedGameDialog.SetActive(true);
        else
            GameManager.Instance.LoadGame();
    }

    // <summary>
    /// Выход из игры (или остановка игры в редакторе).
    /// </summary>
    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    /// <summary>
    /// Устанавливает громкость и обновляет отображение значения.
    /// </summary>
    /// <param name="volume">Значение громкости (от 0 до 1).</param>
    public void SetVolume(float volume)
    {
        CameraSettingsController.Instance.Volume = volume;
        volumeTextValue.text = (volume * 100).ToString("0") + " %";
    }

    /// <summary>
    /// Применяет текущую настройку громкости и сохраняет её.
    /// </summary>
    public void VolumeApply()
    {
        SettingsManager.Instance.SetSetting(
            SettingsKeys.Volume,
            CameraSettingsController.Instance.Volume
        );

        StartCoroutine(ConfirmationBox());
    }

    /// <summary>
    /// Применяет изменения управления и сохраняет их.
    /// </summary>
    public void ControlsApply()
    {
        SettingsManager.Instance.SetSetting(
            SettingsKeys.Rebinds,
            _inputActions.SaveBindingOverridesAsJson()
        );
        StartCoroutine(ConfirmationBox());
    }

    /// <summary>
    /// Устанавливает яркость и обновляет отображение значения.
    /// </summary>
    /// <param name="brightness">Значение яркости.</param>
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.00");
        if (CameraSettingsController.Instance)
            CameraSettingsController.Instance.Brightness = brightness;
    }

    /// <summary>
    /// Устанавливает полноэкранный режим.
    /// </summary>
    /// <param name="isFullScreen">Включён ли полноэкранный режим.</param>
    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    /// <summary>
    /// Устанавливает уровень качества графики.
    /// </summary>
    /// <param name="qualityIndex">Индекс качества.</param>
    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    /// <summary>
    /// Применяет графические настройки и сохраняет их.
    /// </summary>
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

    /// <summary>
    /// Сброс настроек в выбранной категории к значениям по умолчанию.
    /// </summary>
    /// <param name="MenuType">Тип меню: "Graphics", "Audio" или "Controls".</param>
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

    /// <summary>
    /// Показывает анимацию подтверждения применения настроек.
    /// </summary>
    /// <returns>Корутина для анимации.</returns>
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
