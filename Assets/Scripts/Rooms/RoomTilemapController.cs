using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTilemapController : TilemapController
{
    [SerializeField] Optional<Tilemap> _additional;
    [SerializeField] Optional<Tilemap> _hole;
    [SerializeField] Optional<Transform> TrapSpawns;
    [SerializeField] Optional<Transform> DestroySpawns;

    public override void SwapTiles(TilesContainer container)
    {
        base.SwapTiles(container);

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

}
