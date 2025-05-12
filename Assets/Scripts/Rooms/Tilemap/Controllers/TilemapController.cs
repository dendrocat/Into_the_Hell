using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Базовый контроллер для управления тайлмапами. Реализует интерфейс <see cref="ITilemapController"/>.
/// Данный компонент должен быть удалён с игрового объекта после генерации уровня с помощью <see cref="Destroy"/>.
/// </summary>
/// </summary>
public class TilemapController : MonoBehaviour, ITilemapController
{
    [Header("Тайлы для замены")]
    /// <summary>
    /// Контейнер, содержащий шаблонные тайлы, используемые для замены.
    /// </summary>
    [Tooltip("Контейнер с шаблонными тайлами для замены")]
    [SerializeField] protected TilesContainer _templateContainer;

    [Header("Тайлмапы")]
    /// <summary>
    /// Список тайлмапов, представляющих стены объекта.
    /// </summary>
    [Tooltip("Список тайлмапов, представляющих стены объекта")]
    [SerializeField] protected List<Tilemap> _walls;

    /// <summary>
    /// Тайлмап, представляющий пол объекта.
    /// </summary>
    [Tooltip("Тайлмап, представляющий пол объекта")]
    [SerializeField] protected Tilemap _floor;

    /// <inheritdoc />
    public virtual void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
        SwapFloorTiles(container);
    }

    /// <summary>
    /// Заменяет тайлы стен в тайлмапах на тайлы, указанные в переданном контейнере.
    /// </summary>
    /// <param name="container">Контейнер, содержащий тайлы стен для замены.</param>
    protected void SwapWallTiles(TilesContainer container)
    {
        foreach (var wall in _walls)
        {
            wall.SwapTile(_templateContainer.Roof, container.Roof);
            wall.SwapTile(_templateContainer.WallInner, container.WallInner);
            wall.SwapTile(_templateContainer.WallOuter, container.WallOuter);
        }
    }

    /// <summary>
    /// Заменяет тайлы пола в тайлмапе на тайлы, указанные в переданном контейнере.
    /// </summary>
    /// <param name="container">Контейнер, содержащий тайлы пола для замены.</param>
    protected void SwapFloorTiles(TilesContainer container)
    {
        _floor.SwapTile(_templateContainer.Floor, container.Floor);
        _floor.RefreshAllTiles();
    }
}
