using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTilemapController : TilemapController
{
    [SerializeField] Tilemap _wallUp;
    [SerializeField] Tilemap _wallDown;
    [SerializeField] Tilemap _floor;
    [SerializeField] Tilemap _hole;

    public override void SwapTiles(TilesContainer container)
    {
        _wallUp.SwapTile(_templateContainer.WallUp, container.WallUp);
        _wallUp.SwapTile(_templateContainer.WallDownInner, container.WallDownInner);

        _wallDown.SwapTile(_templateContainer.WallUp, container.WallUp);
        _wallDown.SwapTile(_templateContainer.WallDownOuter, container.WallDownOuter);

        _floor.SwapTile(_templateContainer.Floor, container.Floor);

        _hole.SwapTile(_templateContainer.Hole, container.Hole);
    }

}
