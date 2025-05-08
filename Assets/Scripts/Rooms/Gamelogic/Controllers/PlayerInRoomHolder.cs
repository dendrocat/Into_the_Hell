using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInRoomHolder : MonoBehaviour
{
    Player _player;

    Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    void FixPlayerIn()
    {
        Bounds containerBounds = _collider.bounds;
        Bounds targetBounds = _player.GetComponent<Collider2D>().bounds;

        Vector3 dt = Vector3.right + Vector3.up * 2;
        containerBounds.min -= dt - Vector3.up;
        containerBounds.max += dt;

        if (containerBounds.Contains(targetBounds.min) &&
               containerBounds.Contains(targetBounds.max)) return;

        var minBound = containerBounds.min - targetBounds.min;
        var maxBound = containerBounds.max - targetBounds.max;

        dt = Vector3.zero;
        for (int i = 0; i < 3; ++i)
        {
            if (minBound[i] > 0)
                dt[i] += minBound[i] + 1f;
            if (maxBound[i] < 0)
                dt[i] += maxBound[i] - 1f;
        }
        _player.transform.position += dt;
    }

    void FixedUpdate()
    {
        if (_player)
            FixPlayerIn();
    }
}
