using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTilemapController : TilemapController
{
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

}
