using UnityEngine;

public class LevelManager : AbstractLevelManager
{
    Level _level;

    protected override void InitLevel()
    {
        _level = new(new GameObject("Rooms").transform);
    }

    public override void Generate()
    {
        if (_level.created()) return;

        Generator.New()
                .SetUpRooms(_levelStorage.GetRoomContainer(_location), isLastLevel)
                .Halls(_levelStorage.GetHalls())
                .GenerateRooms(15, _level)
                .ActivateLevel(_level,
                    _levelStorage.GetTilesContainer(_location),
                    _levelStorage.GetTrapContainer(_location)
                );

        UIManager.Instance.GenerateMap(_level);
    }
}