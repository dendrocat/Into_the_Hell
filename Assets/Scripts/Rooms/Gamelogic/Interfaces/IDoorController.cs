using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для управления дверями.
/// </summary>
public interface IDoorController
{
    /// <summary>
    /// Устанавливает список всех дверей данной комнаты для управления.
    /// </summary>
    /// <param name="doors">Список объектов, представляющих двери для управления.</param>
    public void SetDoors(List<GameObject> doors);

    /// <summary>
    /// Открывает все двери, управляемые этим контроллером.
    /// </summary>
    public void OpenDoors();

    /// <summary>
    /// Закрывает все двери, управляемые этим контроллером.
    /// </summary>
    public void CloseDoors();
    
    /// <summary>
    /// Удаляет все двери, управляемые этим контроллером.
    /// </summary>
    public void RemoveDoors();
}