using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HallTilemapController : TilemapController, IHallTilemapController
{

    (int, int) getBounds(List<Tilemap> tilemaps)
    {
        int lowerBound = int.MaxValue;
        int upperBound = int.MinValue;
        foreach (var tilemap in tilemaps)
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;
                lowerBound = Mathf.Min(lowerBound, pos.x);
                upperBound = Mathf.Max(upperBound, pos.x);
            }
        }
        return (lowerBound, upperBound);
    }

    public void ExpandHall(int length)
    {
        List<Tilemap> tilemaps = new() { _floor };
        tilemaps.AddRange(_walls);
        int yMin = 0;
        (var lowerBound, var upperBound) = getBounds(tilemaps);

        foreach (var tilemap in tilemaps)
        {
            for (int i = lowerBound; i <= upperBound; ++i)
            {
                var pos = (Vector3Int)new Vector2Int(i, yMin);
                if (!tilemap.HasTile(pos)) continue;
                var tile = tilemap.GetTile(pos);
                List<Vector3Int> poses = new(length - 1);
                for (int y = 1; y < length; y++)
                {
                    poses.Add(new Vector3Int(i, y + yMin, 0));
                }
                tilemap.SetTiles(poses.ToArray(),
                                Enumerable.Repeat<TileBase>(tile, length - 1).ToArray());
            }
        }

        foreach (var tilemap in tilemaps)
        {
            tilemap.RefreshAllTiles();
            if (tilemap.TryGetComponent(out TilemapCollider2D collider2D))
                collider2D.ProcessTilemapChanges();
        }
    }
}
