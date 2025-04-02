/// <summary>
/// Interface for objects that manage tilemaps
/// </summary>
public interface ITilemapController
{
    /// <summary>
    /// Replaces all tiles in the tilemap with the tiles specified in the provided container.
    /// </summary>
    /// <param name="container">The container holding the tiles to replace the existing ones.</param>
    public void SwapTiles(TilesContainer container);
}