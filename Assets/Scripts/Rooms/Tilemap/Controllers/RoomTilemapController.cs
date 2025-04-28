using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for managing a room's tilemap. Implements the <see cref="IRoomTilemapController"/> interface.
/// This component must be removed from the game object after level generation via <see cref="Destroy"/>.
/// Also, it destroys the associated <see cref="IDoorTilemapController"/> instance.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class RoomTilemapController : TilemapController, IRoomTilemapController
{
    /// <summary>
    /// The tilemap for additional elements. This is an optional parameter.
    /// </summary>
    [SerializeField] Optional<Tilemap> _additional;

    /// <summary>
    /// The tilemap for holes. This is an optional parameter.
    /// </summary>
    [SerializeField] Optional<Tilemap> _hole;

    /// <summary>
    /// The transform containing spawn points for traps. This is an optional parameter.
    /// </summary>
    [SerializeField] Optional<Transform> TrapSpawns;

    /// <summary>
    /// The transform containing spawn points for destroyable objects. This is an optional parameter.
    /// </summary>
    [SerializeField] Optional<Transform> DestroySpawns;

    /// <inheritdoc />
    public IDoorTilemapController DoorTilemap { get; private set; }

    /// <inheritdoc />
    public Vector2Int Size { get; private set; }

    /// <summary>
    /// Initializes the door tilemap controller by finding it in the children of this component.
    /// </summary>
    void Awake()
    {
        DoorTilemap = GetComponentInChildren<IDoorTilemapController>();
        var size = GetComponent<BoxCollider2D>().bounds.size;
        Size = new Vector2Int((int)size.x, (int)size.y);
        Size += Vector2Int.up * 5 + Vector2Int.right * 3;
    }

    /// <summary>
    /// Replaces all tiles in the tilemap with the tiles specified in the provided container.
    /// Also updates the door tilemap and optional tilemaps (e.g., holes, traps, destroyable objects).
    /// </summary>
    /// <param name="container">The container holding the tiles and prefabs for replacement.</param>
    public override void SwapTiles(TilesContainer container)
    {
        base.SwapTiles(container);
        DoorTilemap?.SwapTiles(container);

        if (_hole.Enabled)
        {
            _hole.Value.SwapTile(_templateContainer.Hole, container.Hole);
            if (_hole.Value.TryGetComponent(out TilemapCollider2D collider2D))
                collider2D.ProcessTilemapChanges();
        }

        if (TrapSpawns.Enabled)
        {
            CreateInteractable(TrapSpawns.Value, container.Trap);
        }

        if (DestroySpawns.Enabled)
        {
            CreateInteractable(DestroySpawns.Value, container.Destroys);
        }
    }

    /// <summary>
    /// Instantiates game objects at the positions of child transforms of the specified parent.
    /// The new objects are parented to the same transform.
    /// </summary>
    /// <param name="parent">The parent transform containing the spawn points. It also becomes the parent of the new objects.</param>
    /// <param name="prefab">The prefab to instantiate at each spawn point.</param>
    void CreateInteractable(Transform parent, GameObject prefab)
    {
        Transform[] childs = parent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childs.Length; ++i)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = parent;
            obj.transform.localPosition = childs[i].localPosition;
            Destroy(childs[i].gameObject);
        }
    }

    /// <inheritdoc />
    public void RemoveAdditional()
    {
        if (!_additional.Enabled) return;
        Destroy(_additional.Value.gameObject);
        _additional.Enabled = false;
    }

    /// <inheritdoc />
    public List<Vector2Int> GetFreePositions()
    {
        if (!_additional.Enabled) return new List<Vector2Int>();

        var free = new List<Vector2Int>();
        foreach (var pos in _additional.Value.cellBounds.allPositionsWithin)
        {
            if (_additional.Value.GetTile(pos) != null)
                free.Add(new Vector2Int(pos.x, pos.y));
        }
        _additional.Value.ClearAllTiles();
        return free;
    }

    /// <inheritdoc />
    public void SetAdditionalTile(TileBase tile, Vector2Int pos)
    {
        if (!_additional.Enabled) return;
        _additional.Value.SetTile(new Vector3Int(pos.x, pos.y, 0), tile);
        _additional.Value.RefreshAllTiles();
    }


    /// <summary>
    /// Destroys the door tilemap controller when this component is destroyed.
    /// </summary>
    void OnDestroy()
    {
        Destroy(DoorTilemap as MonoBehaviour);
    }
}
