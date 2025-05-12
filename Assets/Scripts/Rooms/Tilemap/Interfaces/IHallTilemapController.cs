/// <summary>
/// Интерфейс контроллера для холловых тайлмапов с возможностью расширения размера.
/// </summary>
public interface IHallTilemapController : ITilemapController, ISizeGetable
{
    /// <summary>
    /// Расширяет структуру холла до указанной длины (в тайлах).
    /// </summary>
    /// <param name="length">Цклевая длина в тайлах.</param>
    public void ExtendToLength(int length);
}
