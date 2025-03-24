using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    [Header("For sizing")]
    [SerializeField] bool showGizmos;

    [SerializeField] Vector2 size;

    [Header("For tiles swapping")]
    [SerializeField] protected TilesContainer _templateContainer;

    [SerializeField] protected List<Tilemap> _walls;

    [SerializeField] protected Tilemap _floor;


    public virtual void SwapTiles(TilesContainer container)
    {
        foreach (var wall in _walls)
        {
            wall.SwapTile(_templateContainer.WallUp, container.WallUp);
            wall.SwapTile(_templateContainer.WallDownInner, container.WallDownInner);
            wall.SwapTile(_templateContainer.WallDownOuter, container.WallDownOuter);
        }
        _floor.SwapTile(_templateContainer.Floor, container.Floor);
        _floor.RefreshAllTiles();
    }


    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        Gizmos.DrawCube(transform.position, size);
    }
}
