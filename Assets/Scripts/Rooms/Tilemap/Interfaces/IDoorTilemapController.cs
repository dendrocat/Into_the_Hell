using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для управления тайлмапом дверей.
/// </summary>
public interface IDoorTilemapController : ITilemapController
{
    /// <summary>
    /// Получает список активных <see cref="GameObject"/> дверей в комнате.
    /// Данное свойство предоставляет доступ ко всем активным экземплярам дверей,
    /// позволяя выполнять их дальнейшее управление или отслеживание состояний во время игры.
    /// </summary>
    public List<GameObject> ActiveDoors { get; }

    /// <summary>
    /// Активирует дверь в указанном направлении.
    /// </summary>
    /// <param name="direction">Направление, указывающее расположение двери для активации.</param>
    public void ActivateDoor(DoorDirection direction);
}