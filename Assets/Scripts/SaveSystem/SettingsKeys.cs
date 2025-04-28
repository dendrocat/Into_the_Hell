/// <summary>
/// Defines standard game setting identifiers for use with <see cref="SettingsRepository"/>.
/// </summary>
/// <remarks>
/// Represents common game configuration parameters managed by <see cref="SettingsManager"/>.
/// Use with <see cref="SettingsRepository.Save"/> and <see cref="SettingsRepository.Load"/>.
/// </remarks>
public enum SettingsKeys
{
    /// <summary>
    /// Audio volume level (float 0.0-1.0)
    /// </summary>
    Volume,


    /// <summary>
    /// Y-axis inversion toggle (bool)
    /// </summary>
    /// 
    InvertY,
    /// <summary>
    /// Input sensitivity multiplier (float)
    /// </summary>
    Sensitivity,


    /// <summary>
    /// Screen brightness level (float 0.0-1.0)
    /// </summary>
    Brightness,
    /// <summary>
    /// Graphics quality preset (int)
    /// </summary>
    Quality,
    /// <summary>
    /// Fullscreen mode toggle (bool)
    /// </summary>
    FullScreen,

    /// <summary>
    /// Display resolution
    /// </summary>
    Resolution
}