using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Controller for hall tilemaps with size extension capabilities</summary>
public class HallTilemapController : TilemapController, IHallTilemapController
{
    /// <summary>Hall orientation axis</summary>
    [Header("Hall Direction")]
    [SerializeField] HallDirection _direction;

    /// <summary>Tile boundaries in world coordinates</summary>
    Vector2Int _lowerBound, _upperBound;

    /// <summary>Container for managed tilemaps</summary>
    List<Tilemap> _tilemaps;

    /// <inheritdoc />
    public Vector2Int Size { get; private set; }

    /// <summary>Calculates current size from tilemap bounds</summary>
    void UpdateSize()
    {
        var size = new Vector2Int(_upperBound.x - _lowerBound.x + 1,
                                    _upperBound.y - _lowerBound.y + 1);
        // if (_direction == HallDirection.Horizontal)
        //     size.Set(size.y, size.x);
        Size = size;
    }

    /// <summary>Finds extreme coordinates across all managed tilemaps</summary>
    /// <param name="tilemaps">Tilemaps to analyze (floor + walls)</param>
    void getBounds(List<Tilemap> tilemaps)
    {
        _lowerBound = new Vector2Int(int.MaxValue, int.MaxValue);
        _upperBound = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var tilemap in tilemaps)
        {
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;
                _lowerBound.x = Mathf.Min(_lowerBound.x, pos.x);
                _upperBound.x = Mathf.Max(_upperBound.x, pos.x);
                _lowerBound.y = Mathf.Min(_lowerBound.y, pos.y);
                _upperBound.y = Mathf.Max(_upperBound.y, pos.y);
            }
        }
    }

    /// <summary>Initialization hook for tilemap analysis</summary>
    void Awake()
    {
        _tilemaps = new() { _floor };
        _tilemaps.AddRange(_walls);
        getBounds(_tilemaps);
        UpdateSize();
    }

    /// <summary>Creates a 3D position using axis values based on corridor orientation</summary>
    /// <param name="main">Primary axis coordinate (X for horizontal, Y for vertical)</param>
    /// <param name="cross">Perpendicular axis coordinate</param>
    /// <returns>Vector3Int with configured coordinates and zero Z-axis</returns>
    Vector3Int CreatePose(int main, int cross)
    {
        return _direction == HallDirection.Horizontal
                ? new Vector3Int(main, cross, 0)
                : new Vector3Int(cross, main, 0);
    }

    /// <summary>Updates either X or Y coordinates in positions based on corridor direction</summary>
    /// <param name="poses">Collection of tile positions to modify</param>
    /// <param name="value">
    /// Coordinate value to apply: 
    /// <list type="bullet">
    /// <item><description>Y-axis for horizontal corridors</description></item>
    /// <item><description>X-axis for vertical corridors</description></item>
    /// </list>
    /// </param>
    void UpdatePoses(List<Vector3Int> poses, int value)
    {
        bool isHorizontal = _direction == HallDirection.Horizontal;

        for (int i = 0; i < poses.Count; ++i)
        {
            var pos = poses[i];
            if (isHorizontal)
                pos.y = value;
            else
                pos.x = value;
            poses[i] = pos;
        }
    }

    /// <inheritdoc />
    public void ExtendToLength(int length)
    {
        bool isHorizontal = _direction == HallDirection.Horizontal;
        var (mainSize, mainUpper) = isHorizontal
            ? (Size.x, _upperBound.x)
            : (Size.y, _upperBound.y);

        var (crossLower, crossUpper) = isHorizontal
            ? (_lowerBound.y, _upperBound.y)
            : (_lowerBound.x, _upperBound.x);

        length -= mainSize;
        if (length <= 0) return;

        var tiles = new TileBase[length];
        var poses = Enumerable.Range(1, length)
                    .Select(i => CreatePose(mainUpper + i, crossUpper))
                    .ToList();

        foreach (var tilemap in _tilemaps)
        {
            for (int cross = crossLower; cross <= crossUpper; ++cross)
            {
                var basePos = CreatePose(mainUpper, cross);
                if (!tilemap.HasTile(basePos)) continue;

                var tile = tilemap.GetTile(basePos);
                UpdatePoses(poses, cross);

                Array.Fill(tiles, tile);
                tilemap.SetTiles(poses.ToArray(), tiles);
            }
        }

        if (isHorizontal)
            _upperBound.x += length;
        else
            _upperBound.y += length;

        UpdateSize();

        foreach (var tilemap in _tilemaps)
        {
            tilemap.RefreshAllTiles();
            if (tilemap.TryGetComponent(out TilemapCollider2D collider2D))
                collider2D.ProcessTilemapChanges();
        }
    }
}
