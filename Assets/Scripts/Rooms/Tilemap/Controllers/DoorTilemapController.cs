using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Контроллер для управления тайлмапами стен с дверями. Реализует интерфейс <see cref="IDoorTilemapController"/>.
/// </summary>
public class DoorTilemapController : TilemapController, IDoorTilemapController
{
    /// <summary>
    /// Список игровых объектов дверей в комнате.
    /// </summary>
    [Tooltip("Список игровых объектов дверей в комнате")]
    [SerializeField] List<GameObject> _doors;

    /// <inheritdoc />
    public List<GameObject> ActiveDoors { get; private set; }

    /// <summary>
    /// Инициализация контроллера, создание списка активных дверей.
    /// </summary>
    void Awake()
    {
        ActiveDoors = new List<GameObject>();
    }

    /// <inheritdoc />
    public override void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
        SwapDoorTiles(container);
    }

    /// <summary>
    /// Заменяет все тайлы в тайлмапах дверей на тайлы из переданного контейнера.
    /// </summary>
    /// <param name="container">Контейнер с тайлами для замены.</param>
    void SwapDoorTiles(TilesContainer container)
    {
        foreach (var door in _doors)
        {
            var tilemaps = door.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tilemaps)
            {
                tilemap.SwapTile(_templateContainer.DoorRoof, container.DoorRoof);
                tilemap.SwapTile(_templateContainer.DoorWall, container.DoorWall);
            }
        }
    }

    /// <inheritdoc />
    public void ActivateDoor(DoorDirection door)
    {
        _doors[(int)door].SetActive(true);
        ActiveDoors.Add(_doors[(int)door]);

        _walls[(int)door].transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Очищает неактивные двери и тайлмапы стен при уничтожении компонента.
    /// </summary>
    void OnDestroy()
    {
        _doors.RemoveAll((el) =>
            {
                if (!ActiveDoors.Contains(el))
                {
                    Destroy(el);
                    return true;
                }
                return false;
            }
        );
        _walls.RemoveAll((el) =>
            {
                var a = el.transform.parent.gameObject;
                if (!a.activeInHierarchy)
                {
                    Destroy(a);
                    return true;
                }
                return false;
            }
        );
    }
}
