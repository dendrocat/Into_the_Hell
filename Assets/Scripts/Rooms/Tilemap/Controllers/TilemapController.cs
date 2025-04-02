using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Base controller for managing tilemaps. Implements the <see cref="ITilemapController"/> interface.
/// This component must be removed from the game object after level generation via <see cref="Destroy"/>.
/// </summary>
public class TilemapController : MonoBehaviour, ITilemapController
{
    /// <summary>
    /// Container holding the template tiles used for swapping.
    /// </summary>
    [Header("Tiles swapping")]
    [SerializeField] protected TilesContainer _templateContainer;

    /// <summary>
    /// List of tilemaps representing the walls of the object.
    /// </summary>
    [SerializeField] protected List<Tilemap> _walls;

    /// <summary>
    /// Tilemap representing the floor of the object.
    /// </summary>
    [SerializeField] protected Tilemap _floor;

    /// <inheritdoc />
    public virtual void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
        SwapFloorTiles(container);
    }

    /// <summary>
    /// Replaces wall tiles in the tilemaps with those specified in the provided container.
    /// </summary>
    /// <param name="container">The container holding the wall tiles for replacement.</param>
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
    /// Replaces floor tiles in the tilemap with those specified in the provided container.
    /// </summary>
    /// <param name="container">The container holding the floor tiles for replacement.</param>
    protected void SwapFloorTiles(TilesContainer container)
    {
        _floor.SwapTile(_templateContainer.Floor, container.Floor);
        _floor.RefreshAllTiles();
    }
}
