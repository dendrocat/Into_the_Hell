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
    List<GameObject> _doors;

    /// <summary>
    /// Initializes the controller by gathering all tilemaps and door game objects in the children of this component.
    /// </summary>
    void Awake()
    {
        _walls = new List<Tilemap>(GetComponentsInChildren<Tilemap>());
        _doors = new List<GameObject>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var d = transform.GetChild(i);
            if (d.GetComponent<Tilemap>() != null)
                continue;
            _doors.Add(d.gameObject);
        }
    }

    /// <inheritdoc />
    public override void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
    }

    /// <inheritdoc />
    public void ActivateDoor(DoorDirection door)
    {
        _doors[(int)door].SetActive(true);
        _walls[(int)door].gameObject.SetActive(false);
    }

    /// <summary>
    /// Cleans up inactive doors and wall tilemaps when this component is destroyed.
    /// </summary>
    void OnDestroy()
    {
        _doors.RemoveAll((el) =>
            {
                if (!el.activeSelf)
                {
                    Destroy(el);
                    return true;
                }
                return false;
            }
        );
        _walls.RemoveAll((el) =>
            {
                if (!el.isActiveAndEnabled)
                {
                    Destroy(el.gameObject);
                    return true;
                }
                return false;
            }
        );
    }
}
