/// <summary>
/// Interface for controlling a room, providing access to additional effects and door management.
/// </summary>
public interface IRoomController
{
    /// <summary>
    /// Gets the controller for managing additional effects or modifications within the room.
    /// </summary>
    /// <value>
    /// An <see cref="IAdditionalController"/> instance responsible for applying additional effects.
    /// </value>
    public IAdditionalController AdditionalController { get; }

    /// <summary>
    /// Gets the controller for managing doors within the room.
    /// </summary>
    /// <value>
    /// An <see cref="IDoorController"/> instance responsible for finding, opening, and closing doors.
    /// </value>
    public IDoorController DoorController { get; }

    public void MarkAsEndRoom();

    public void ActivateRoom();
}