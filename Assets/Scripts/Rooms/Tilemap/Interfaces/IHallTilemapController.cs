/// <summary>Controller interface for hall tilemaps requiring size extension capabilities</summary>
public interface IHallTilemapController : ITilemapController, ISizeGetable
{
    /// <summary>Extends hall structure to specified length</summary>
    /// <param name="length">Target length in tiles</param>
    public void ExtendToLength(int length);
}
