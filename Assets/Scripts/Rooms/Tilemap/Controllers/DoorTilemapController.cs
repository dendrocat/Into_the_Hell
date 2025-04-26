using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for managing tilemaps of walls with doors. Implements the <see cref="IDoorTilemapController"/> interface.
/// </summary>
public class DoorTilemapController : TilemapController, IDoorTilemapController
{
    /// <summary>
    /// List of door game objects in the room.
    /// </summary>
    [SerializeField] List<GameObject> _doors;

    /// <inheritdoc />
    public List<GameObject> ActiveDoors { get; private set; }

    /// <summary>
    /// Initializes the controller by gathering all tilemaps and door game objects in the children of this component.
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

    /// <summary>Replaces all tiles in the door tilemaps with the tiles specified in the provided container. </summary>
    /// <param name="container">The container holding the tiles to replace the existing ones.</param>
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
    /// Cleans up inactive doors and wall tilemaps when this component is destroyed.
    /// </summary>
    void OnDestroy()
    {
        _doors.RemoveAll((el) =>
            {
                if (!el.activeInHierarchy)
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
