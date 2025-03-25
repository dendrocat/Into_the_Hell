using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for tilemaps of walls with doors.
/// Can delete some walls and set doors instead
/// Must be removed from gameObject after level generation via DestroyUnused
/// </summary>
public class DoorTilemapController : TilemapController
{
    public UnityEvent<List<GameObject>> OnDeletedUnusedDoors;
    List<GameObject> _doors;

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


    public override void SwapTiles(TilesContainer container)
    {
        SwapWallTiles(container);
    }

    public void ActivateDoor(DoorDirection door)
    {
        _doors[(int)door].SetActive(true);
        _walls[(int)door].gameObject.SetActive(false);
    }

    public override void DestroyUnused()
    {
        _doors.RemoveAll((el) =>
        {
            if (!el.activeSelf)
            {
                Destroy(el);
                return true;
            }
            return false;
        });

        //base.DestroyUnused();
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return null;

        OnDeletedUnusedDoors.Invoke(_doors);
        OnDeletedUnusedDoors.RemoveAllListeners();
        OnDeletedUnusedDoors = null;

        base.DestroyUnused();
    }
}
