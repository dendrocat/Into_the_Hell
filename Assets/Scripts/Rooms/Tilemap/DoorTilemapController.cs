using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for tilemaps of walls with doors.
/// Can delete some walls and set doors instead
/// Must be removed from gameObject after level generation
/// </summary>
public class DoorTilemapController : TilemapController
{
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
        _doors.ForEach((el) =>
        {
            if (!el.activeSelf)
                Destroy(el);
        });
        _doors.RemoveAll((el) => el.IsDestroyed());

        _walls.ForEach((el) =>
        {
            if (!el.isActiveAndEnabled)
                Destroy(el.gameObject);
        });
        _walls.RemoveAll((el) => el.IsDestroyed());

        base.DestroyUnused();
    }
}
