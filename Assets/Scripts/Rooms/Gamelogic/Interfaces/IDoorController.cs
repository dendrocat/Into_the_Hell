/// <summary>
/// Interface for controlling doors.
/// </summary>
public interface IDoorController
{
    /// <summary>
    /// Finds all doors tagged with "Door" in the children of this GameObject.
    /// </summary>
    /// <remarks>
    /// This method is typically used during generation to locate door objects in the scene.
    /// </remarks>
    public void FindDoors();

    /// <summary>
    /// Opens all doors managed by this controller.
    /// </summary>
    /// <remarks>
    /// The implementation should handle the logic for visually and functionally opening the doors.
    /// </remarks>
    public void OpenDoors();

    /// <summary>
    /// Closes all doors managed by this controller.
    /// </summary>
    /// <remarks>
    /// The implementation should handle the logic for visually and functionally closing the doors.
    /// </remarks>
    public void CloseDoors();
}