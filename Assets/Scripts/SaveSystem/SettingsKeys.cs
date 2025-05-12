/// <summary>
/// Определяет стандартные идентификаторы настроек игры для использования с <see cref="SettingsRepository"/>.
/// </summary>
/// <remarks>
/// Представляет общие параметры конфигурации игры, управляемые <see cref="SettingsManager"/>.
/// Используется с методами <see cref="SettingsRepository.Save"/> и <see cref="SettingsRepository.Load"/>.
/// </remarks>
public enum SettingsKeys
{
    /// <summary>
    /// Уровень громкости звука (<see langword="float"/> от 0.0 до 1.0).
    /// </summary>
    Volume,

    /// <summary>
    /// Уровень яркости экрана (<see langword="float"/> от 0.0 до 1.0).
    /// </summary>
    Brightness,

    /// <summary>
    /// Предустановка качества графики (<see langword="int"/>).
    /// </summary>
    Quality,

    /// <summary>
    /// Полноэкранный режимм (<see langword="bool"/>).
    /// </summary>
    FullScreen,

    /// <summary>
    /// Разрешение экрана (<see langword="int"/>).
    /// </summary>
    Resolution,

    /// <summary>
    /// Настройки переназначения клавиш (<see langword="string"/>).
    /// </summary>
    Rebinds
}