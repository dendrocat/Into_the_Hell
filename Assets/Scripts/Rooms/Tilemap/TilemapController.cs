using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// BaseController for tilemaps.
/// Can swap wall and floor tiles
/// Must be removed from gameObject after level generation via DestroyUnused
/// </summary>
public class TilemapController : MonoBehaviour
{
    [Header("Tiles swapping")]
    [SerializeField] protected TilesContainer _templateContainer;

    [SerializeField] protected List<Tilemap> _walls;

    [SerializeField] protected Tilemap _floor;


    public virtual void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
        SwapFloorTiles(container);
    }

    protected void SwapWallTiles(TilesContainer container)
    {
        foreach (var wall in _walls)
        {
            wall.SwapTile(_templateContainer.WallUp, container.WallUp);
            wall.SwapTile(_templateContainer.WallDownInner, container.WallDownInner);
            wall.SwapTile(_templateContainer.WallDownOuter, container.WallDownOuter);
        }
    }

    protected void SwapFloorTiles(TilesContainer container)
    {
        _floor.SwapTile(_templateContainer.Floor, container.Floor);
        _floor.RefreshAllTiles();
    }

    public virtual void DestroyUnused()
    {
        Destroy(this);
    }
}
