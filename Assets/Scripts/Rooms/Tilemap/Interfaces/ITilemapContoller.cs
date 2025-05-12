/// <summary>
/// Интерфейс для объектов, управляющих тайлмапами.
/// </summary>
public interface ITilemapController
{
/// <summary>
    /// Заменяет все тайлы в тайлмапе на тайлы, указанные в переданном контейнере.
    /// </summary>
    /// <param name="container"><see cref="TilesContainer">Контейнер</see>, содержащий тайлы для замены существующих.</param>
    public void SwapTiles(TilesContainer container);
}