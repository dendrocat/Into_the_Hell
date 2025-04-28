using UnityEngine;

public class BaseRoomManager : MonoBehaviour
{
    public static BaseRoomManager Instance { get; private set; } = null;

    [SerializeField] RoomTilemapController _baseRoom;

    void Awake()
    {
        Instance = this;
    }

    public void SwapTiles(Locations location)
    {
        _baseRoom.SwapTiles(
            LevelStorage.Instance.GetTilesContainer(location)
        );
        Instance = null;
        Destroy(LevelStorage.Instance.gameObject);
        Destroy(_baseRoom);
        Destroy(this);
    }
}
