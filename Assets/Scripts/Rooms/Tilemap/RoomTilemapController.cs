using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for room tilemap.
/// Can manage DoorTilemapContoller and set some additional tile and gameObjects
/// Must be removed from gameObject after level generation
/// </summary>
public class RoomTilemapController : TilemapController
{
    [SerializeField] Optional<Tilemap> _additional;
    [SerializeField] Optional<Tilemap> _hole;
    [SerializeField] Optional<Transform> TrapSpawns;
    [SerializeField] Optional<Transform> DestroySpawns;

    DoorTilemapController _doorTilemap;

    void Start()
    {
        _doorTilemap = GetComponentInChildren<DoorTilemapController>();
    }

    public override void SwapTiles(TilesContainer container)
    {
        base.SwapTiles(container);
        _doorTilemap.SwapTiles(container);

        if (_hole.Enabled)
        {
            _hole.Value.SwapTile(_templateContainer.Hole, container.Hole);
        }

        if (TrapSpawns.Enabled)
        {
            CreateInteracts(TrapSpawns.Value, container.Trap);
        }

        if (DestroySpawns.Enabled)
        {
            CreateInteracts(DestroySpawns.Value, container.Destroys);
        }
    }

    void CreateInteracts(Transform parent, GameObject trap)
    {
        Transform[] childs = parent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childs.Length; ++i)
        {
            var obj = Instantiate(trap);
            obj.transform.parent = parent;
            obj.transform.localPosition = childs[i].localPosition;
            Destroy(childs[i].gameObject);
        }
    }

    public List<Vector3Int> GetFreeAdditional()
    {
        if (!_additional.Enabled) return new List<Vector3Int>();

        var free = new List<Vector3Int>();
        foreach (var pos in _additional.Value.cellBounds.allPositionsWithin)
        {
            if (_additional.Value.GetTile(pos) != null)
                free.Add(pos);
        }
        _additional.Value.ClearAllTiles();
        return free;
    }

    public void SetAdditionalTile(Vector3Int pos, TileBase tile)
    {
        _additional.Value.SetTile(pos, tile);
        _additional.Value.RefreshAllTiles();
    }

    public void ActivateDoor(DoorDirection door)
    {
        _doorTilemap.ActivateDoor(door);
    }

    public override void DestroyUnused()
    {
        _doorTilemap.DestroyUnused();

        base.DestroyUnused();
    }



    public void SetAdditionalEffects()
    {
        throw new NotImplementedException("Not implemented setting effects like lava and ice");
    }

}
