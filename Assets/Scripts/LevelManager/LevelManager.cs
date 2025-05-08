using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour, ILevelManager {
    private LevelStorage _levelStorage;

    public static LevelManager Instance { get; private set; } = null;
    Level _level;

    Locations _location;

    void Awake() {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        _levelStorage = LevelStorage.Instance;
        _level = new(new GameObject("Rooms").transform);
    }

    private List<GameObject> getHalls(){
        return _levelStorage.GetHalls();
    }

    private RoomContainer getRoomContainer(){
        return _levelStorage.GetRoomContainer(_location);
    }
    public void Generate(Locations location) {
        _levelStorage = LevelStorage.Instance;
        if (_level.created()) return;

        Generator.New()
                .SetUpRooms(getRoomContainer(), GameStorage.Instance.isLastLevel)
                .Halls(getHalls())
                .GenerateRooms(15, _level)
                .ActivateLevel(_level, 
                    _levelStorage.GetTilesContainer(location),
                    _levelStorage.GetTrapContainer(location) );
        UIManager.Instance.GenerateMap(_level);
        Destroy(LevelStorage.Instance.gameObject);
    }

    void OnDestroy() {
    }
}