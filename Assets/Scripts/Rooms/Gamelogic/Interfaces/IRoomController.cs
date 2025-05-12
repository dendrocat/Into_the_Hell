/// <summary>
/// Интерфейс для управления комнатой, предоставляющий доступ к дополнительным эффектам и управлению дверями.
/// </summary>
public interface IRoomController
{
    /// <summary>
    /// Контроллер для управления дополнительными эффектами или модификациями внутри комнаты.
    /// </summary>
    public IAdditionalController AdditionalController { get; }

    /// <summary>
    /// Контроллер для управления дверями внутри комнаты.
    /// </summary>
    public IDoorController DoorController { get; }

    // <summary>
    /// Помечает комнату как конечную (например, комнату босса или выход).
    /// </summary>
    public void MarkAsEndRoom();

    /// <summary>
    /// Активирует комнату, выполняя необходимые действия для её включения в игру.
    /// </summary>
    public void ActivateRoom();
}