using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Контроллер для управления тайлмапами комнаты. Реализует интерфейс <see cref="IRoomTilemapController"/>.
/// Этот компонент должен быть удалён с игрового объекта после генерации уровня через <see cref="Destroy"/>.
/// Также уничтожает связанный экземпляр <see cref="IDoorTilemapController"/>.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class RoomTilemapController : TilemapController, IRoomTilemapController
{
     [Header("Настройки комнаты")]
    /// <summary>
    /// Тайлмап для дополнительных ловушек.
    /// </summary>
    [Tooltip("Тайлмап для дополнительных ловушек")]
    [SerializeField] Optional<Tilemap> _additional;

    /// <summary>
    /// Точки спавна ловушек (опционально).
    /// </summary>
    [Tooltip("Контейнер точек спавна ловушек")]
    [SerializeField] Optional<Transform> TrapSpawns;

    /// <summary>
    /// Точки спавна разрушаемых объектов (опционально).
    /// </summary>
    [Tooltip("Контейнер точек спавна разрушаемых объектов")]
    [SerializeField] Optional<Transform> DestroySpawns;

    /// <inheritdoc />
    public IDoorTilemapController DoorTilemap { get; private set; }

    /// <inheritdoc />
    public Vector2Int Size { get; private set; }

    /// <summary>
    /// Инициализирует контроллер дверей и вычисляет размер комнаты.
    /// </summary>
    void Awake()
    {
        DoorTilemap = GetComponentInChildren<IDoorTilemapController>();
        var size = GetComponent<BoxCollider2D>().bounds.size;
        Size = new Vector2Int((int)size.x, (int)size.y);
        Size += Vector2Int.up * 5 + Vector2Int.right * 3;
    }

    /// <inheritdoc />
    public override void SwapTiles(TilesContainer container)
    {
        base.SwapTiles(container);
        DoorTilemap?.SwapTiles(container);

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
    /// Создаёт интерактивные объекты на основе префабов в указанных точках спавна.
    /// </summary>
    /// <param name="parent">Родительский трансформ с точками спавна</param>
    /// <param name="prefab">Префаб для создания</param>
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
    /// Уничтожает контроллер дверей при удалении компонента.
    /// </summary>
    void OnDestroy()
    {
        Destroy(DoorTilemap as MonoBehaviour);
    }
}
