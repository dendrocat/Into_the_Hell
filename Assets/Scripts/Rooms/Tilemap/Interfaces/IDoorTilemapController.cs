/// <summary>
/// Interface for controlling the tilemap of doors
/// </summary>
public interface IDoorTilemapController : ITilemapController
{
    /// <summary>
    /// Activates a door at the specified location based on the given direction.
    /// </summary>
    /// <param name="direction">The direction indicating the location of the door to activate.</param>
    public void ActivateDoor(DoorDirection direction);
}