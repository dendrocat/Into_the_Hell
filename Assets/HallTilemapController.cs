using UnityEngine;
using UnityEngine.Tilemaps;

public class HallTilemapController : TilemapController
{
    [SerializeField] Tilemap _wall;
    [SerializeField] Tilemap _floor;

    public override void SwapTiles(TilesContainer container)
    {
        _wall.SwapTile(_templateContainer.WallUp, container.WallUp);
        _floor.SwapTile(_templateContainer.Floor, container.Floor);
    }

}
