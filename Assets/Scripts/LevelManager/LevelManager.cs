using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AbstractLevelManager
{
    Level _level;

    protected override void InitLevel()
    {
        _level = new(new GameObject("Rooms").transform);
    }

    private List<GameObject> getHalls()
    {
        return _levelStorage.GetHalls();
    }

    private RoomContainer getRoomContainer()
    {
        return _levelStorage.GetRoomContainer(_location);
    }

    public override void Generate()
    {
        if (_level.created()) return;

        Generator.New()
                .SetUpRooms(getRoomContainer(), isLastLevel)
                .Halls(getHalls())
                .GenerateRooms(15, _level)
                .ActivateLevel(_level,
                    _levelStorage.GetTilesContainer(_location),
                    _levelStorage.GetTrapContainer(_location)
                );

        UIManager.Instance.GenerateMap(_level);
    }
}