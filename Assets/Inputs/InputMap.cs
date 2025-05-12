/// <summary>
/// Перечисление, определяющее различные схемы управления (карты ввода) в игре.
/// </summary>
/// <remarks>
/// Используется для переключения между следующими режимами ввода:
/// <list type="bullet">
///   <item><description><see cref="Gameplay"/> - схема ввода для основного игрового процесса.</description></item>
///   <item><description><see cref="UI"/> - схема ввода для взаимодействия с пользовательским интерфейсом.</description></item>
/// </list>
/// </remarks>
public enum InputMap
{
    /// <summary>
    /// Схема ввода для игрового процесса.
    /// </summary>
    Gameplay, 
    
    /// <summary>
    /// Схема ввода для пользовательского интерфейса.
    /// </summary>
    UI
}