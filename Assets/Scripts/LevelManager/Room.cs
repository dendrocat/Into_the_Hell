using UnityEngine;

/// <summary>
/// Класс Комнаты. Используется чтобы хранить комнтату для генератора и уровня
/// </summary>
public class Room {

  /// <summary> Ширина комнаты </summary>
	public int w { get; private set; }
  /// <summary> Высота комнаты </summary>
	public int h { get; private set; }
  /// <summary> Сам объект комнаты в Юнити </summary>
	private GameObject room;
  /// <summary> Показывает является ли комната последней на уровне </summary>
	private bool _isEndRoom;
  /// <summary> Показывает является ли комната начальной на уровне </summary>
	public bool _isStartRoom{ get; private set; }
  /// <summary> Координата комнаты </summary>
	public Vector2 coord;


  /// <summary> Конструктор класса </summary>
  /// <param name="prefab"> Префаб комнаты для ее создания <see cref="GameObject"></param>
  /// <param name="level"> Уровень, на котором находиться комната <see cref="Level"></param>
  /// <param name="isEndRoom"> Является ли комната конечной </param>
  /// <param name="isStartRoom"> Является ли комната конечной </param>
	public Room(GameObject prefab, Level level, bool isEndRoom = false, bool isStartRoom = false) {
		room = GameObject.Instantiate(prefab, level.transform);
		var col = room.GetComponent<ISizeGetable>().Size;
		w = col.x;
		h = col.y;
		_isEndRoom = isEndRoom;
		_isStartRoom = isStartRoom;
	}

	public int doors = 0;

  /// <summary> 
  /// Завершающий процесс создания комнаты.
  /// Задает позицию комнаты, активирует двери и параметр для финальной комнаты
  /// </summary>
	public void SetUp() {
		room.transform.position = (Vector3)coord;
		room.GetComponent<IRoomController>().DoorController.SetDoors(
			room.GetComponent<IRoomTilemapController>().DoorTilemap.ActiveDoors);
		if (_isEndRoom)
			room.GetComponent<IRoomController>().MarkAsEndRoom();
	}

  /// <summary> Добавляет дверь в комнтау </summary>
  /// <param name="dir"> Направление в котором нужно создать дверь <see cref="DoorDirection"></param>
	public void AddDoor(DoorDirection dir) {
		doors |= 1 << (int)dir;
		room.GetComponent<IRoomTilemapController>().
				DoorTilemap.ActivateDoor(dir);
	}

  /// <summary> Статический метод, который используется для создания коридора </summary>
  /// <param name="start"> Стратовая позиция коридора <see cref="  Vector2"></param>
  /// <param name="fin"> Конечная позиция коридора <see cref=" Vector2"></param>
    /// <param name="prefab"> Префаб коридора <see cref=" GameObject"></param>
  /// <param name="level"> Уровень, на котором генерируется коридор <see cref=" Level"></param>
	public static void GenerateHall(Vector2 start, Vector2 fin, GameObject prefab, Level level) {
		GameObject hall = GameObject.Instantiate(prefab, level.transform);
		hall.transform.position = (Vector3)start;
		hall.GetComponent<IHallTilemapController>().
				ExtendToLength((int)(fin - start).magnitude);
		level.halls.Add(hall);
	}

  /// <summary> Метод, протаскивающий IRoomTilemapController из комнаты </summary>
  /// <returns> Контроллер <see cref="RoomTilemapController"></returns>
	public IRoomTilemapController GetRoomTilemapController() {
		return room.GetComponent<IRoomTilemapController>();
	}

  /// <summary> Метод, протаскивающий IRoomController из комнаты </summary>
  /// <returns> Контроллер <see cref="RoomController"></returns>
	public IRoomController GetRoomController() {
		return room.GetComponent<IRoomController>();
	}
}
