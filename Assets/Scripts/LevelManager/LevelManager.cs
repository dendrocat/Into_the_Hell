using UnityEngine;

/// <summary> Класс, который отвечает за создание и генерацию уровня </summary>
public class LevelManager : AbstractLevelManager {
  /// <summary> Уровень <see cref="Level"> </summary>
	Level _level;

  /// <summary> Функция инициализации уровня </summary>
	protected override void InitLevel() {
		_level = new(new GameObject("Rooms").transform);
	}

  /// <summary> Функция вызова генерации уровня </summary>
	public override void Generate() {
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
