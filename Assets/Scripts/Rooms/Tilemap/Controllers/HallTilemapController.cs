using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>Controller for hall tilemaps with size extension capabilities</summary>
public class HallTilemapController : TilemapController, IHallTilemapController
{
    /// <summary>Hall orientation axis</summary>
    /// /// <remarks>
    /// Determines coordinate interpretation during extension:
    /// - Vertical: Extends along Y-axis
    /// - Horizontal: Extends along X-axis (automatically converted)
    /// </remarks>
    [Header("Hall Direction")]
    [SerializeField] HallDirection _direction;

    /// <summary>Tile boundaries in world coordinates</summary>
    /// <remarks>
    /// Tracks minimum (_lowerBound) and maximum (_upperBound) coordinates
    /// of all tiles across managed tilemaps. Updated during extension.
    /// </remarks>
    Vector2Int _lowerBound, _upperBound;

    /// <summary>Container for managed tilemaps</summary>
    /// <remarks>
    /// Contains primary floor (_floor) and walls (_walls).
    /// All operations (extension/updates) apply synchronously to all tilemaps.
    /// </remarks>
    List<Tilemap> _tilemaps;

    /// <inheritdoc />
    public Vector2Int Size { get; private set; }

    /// <summary>Calculates current size from tilemap bounds</summary>
    void UpdateSize()
    {
        var size = new Vector2Int(_upperBound.x - _lowerBound.x + 1,
                                    _upperBound.y - _lowerBound.y + 1);
        if (_direction == HallDirection.Horizontal)
            size.Set(size.y, size.x);
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


    /// <summary>Adjusts X-coordinates for new tile positions during extension</summary>
    /// <param name="poses">Position collection to modify</param>
    /// <param name="x">Target X-coordinate for all positions</param>
    void UpdatePoses(List<Vector3Int> poses, int x)
    {
        for (int i = 0; i < poses.Count; ++i)
        {
            var pose = poses[i];
            pose.x = x;
            poses[i] = pose;
        }
    }

    /// <inheritdoc />
    public void ExtendToLength(int length)
    {
        length -= _direction != HallDirection.Horizontal ? Size.y : Size.x;
        if (length <= 0) return;

        var tiles = new TileBase[length];
        List<Vector3Int> poses = new(length);
        for (int y = 1; y <= length; y++)
        {
            poses.Add(new Vector3Int(0, y + _upperBound.y, 0));
        }

        foreach (var tilemap in _tilemaps)
        {
            for (int i = _lowerBound.x; i <= _upperBound.x; ++i)
            {
                var pos = (Vector3Int)new Vector2Int(i, _upperBound.y);
                if (!tilemap.HasTile(pos)) continue;
                var tile = tilemap.GetTile(pos);
                UpdatePoses(poses, i);
                Array.Fill(tiles, tile);
                tilemap.SetTiles(poses.ToArray(), tiles);
            }
        }

        foreach (var tilemap in _tilemaps)
        {
            tilemap.RefreshAllTiles();
            if (tilemap.TryGetComponent(out TilemapCollider2D collider2D))
                collider2D.ProcessTilemapChanges();
        }
        _upperBound.y = _upperBound.y + length;
        UpdateSize();
    }
}
