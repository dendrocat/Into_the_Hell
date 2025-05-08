using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Класс генератора. Используется для полной генерации уровня, включая установку комнат, коридоров и т.д.
/// </summary>
public class Generator {

  /// <summary> Префабы комнат, используемые для генерации </summary>
	private List<GameObject> rooms = null;

  /// <summary>
  /// Префабы коридоров, используемые для генерации
  /// nhall = north hall - префаб вертикального коридора
  /// ehall = east hall - префаб горизонтального коридора
  /// </summary>
	private GameObject nhall = null, ehall = null;

  /// <summary> Префаб первой комнаты </summary>
	private GameObject firstRoom = null;

  /// <summary> Префаб последней комнаты </summary>
	private GameObject lastRoom = null;

  /// <summary> Функция инициации цепочки вызовов генерации </summary>
  /// <returns> Generator - используется в цепочке </returns>
	public static Generator New() { return new(); }

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префабы комнат для генерации.
  /// </summary>
  /// <param name="room_prefabs"> Список префабов <see cref="GameObject"> комнат </param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator Rooms(List<GameObject> room_prefabs) {
		rooms = room_prefabs;
		return this;
	}

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префабы коридоров.
  /// </summary>
  /// <param name="halls"> Список префабов <see cref="GameObject"> коридоров
  ///  Коридоров должно быть два: {вертикальный, горизонтальный}
  /// </param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator Halls(List<GameObject> halls) { return this.Halls(halls[0], halls[1]); }

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префабы коридоров.
  /// </summary>
  /// <param name="nhall_prefab"> Префаб вертикального коридора <see cref="GameObject"> </param>
  /// <param name="ehall_prefab"> Префаб горизонтального коридора <see cref="GameObject"> </param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator Halls(GameObject nhall_prefab, GameObject ehall_prefab) {
		nhall = nhall_prefab; ehall = ehall_prefab;
		return this;
	}

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префаб первой комнаты.
  /// </summary>
  /// <param name="firstRoom_prefab"> Префаб первой комнаты <see cref="GameObject"></param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator FirstRoom(GameObject firstRoom_prefab) {
		firstRoom = firstRoom_prefab;
		return this;
	}

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префаб последней комнаты.
  /// </summary>
  /// <param name="lastRoom_prefab"> Префаб последней комнаты <see cref="GameObject"></param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator LastRoom(GameObject lastRoom_prefab) {
		lastRoom = lastRoom_prefab;
		return this;
	}

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Задает префабы комнат (включая конечную и начальную)
  /// </summary>
  /// <param name="roomContainer"> Контейнер с комнатами <see cref="RoomContainer"></param>
  /// <param name="isBossLevel"> Параметр указывает на то, является ли уровень
  ///     последним на локации (содержит босса) </param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator SetUpRooms(RoomContainer roomContainer, bool isBossLevel) {
		return Rooms(roomContainer.Rooms)
			.FirstRoom(roomContainer.StartRoom)
			.LastRoom(isBossLevel? roomContainer.BossRoom: roomContainer.StartRoom);
	}

  /// <summary> Функция для вывода ошибок генератора </summary>
  /// <param name="message"> Сообщение для вывода </param>
	private void Exc(String message) {
		throw new Exception("Generator: " + message);
	}

  /// <summary>
  /// Часть цепочки вызовов генератора.
  /// Генерирует начальную карту комнат и переходы между ними.
  /// При этом сами комнаты все еще требуют активации
  /// </summary>
  /// <param name="roomNumber"> Количество комнат на уровне </param>
  /// <param name="level"> Уровень, на котором размещаются комнтаты и коридоры <see cref="Level"></param>
  /// <returns> Generator - используется в цепочке </returns>
	public Generator GenerateRooms(int roomNumber, Level level) {
		if (rooms == null) Exc("Rooms are not specified");
		if (nhall == null || ehall == null) Exc("Halls are not specified");
		if (roomNumber < 2 || roomNumber > 999) Exc("Bad room number");

		// Generating positions and coridors;
		List<Vector2Int> possPos = new() { new(0, 0) };
		HashSet<Vector2Int> poses = new();
		System.Random random = new();
		List<Vector2> coridors = new();

		Vector2Int firstRoom_coord = new(-1000, -1000), lastRoom_coord = firstRoom_coord;
		while (roomNumber-- > 0) {
			int ind = random.Next(1000007);
			ind %= possPos.Count;
			Vector2Int pos = possPos[ind];
			possPos[ind] = possPos[possPos.Count - 1];
			possPos.RemoveAt(possPos.Count - 1);
			if (poses.Contains(pos)) {
				++roomNumber;
			}
			else
			{
				poses.Add(pos);
				lastRoom_coord = pos;
				if (firstRoom_coord.x == -1000) firstRoom_coord = pos;
				int num = 0;
				Vector2 coridor = new(0f, 0f);
				Array.ForEach(new Vector2Int[]{
					new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
				}, (shift) =>
				{
					if (poses.Contains(shift + pos)) {
						if (UnityEngine.Random.value <= 1f / ++num || coridor == Vector2.zero)
							coridor = pos + 0.5f * (Vector2)shift;
					}
					else possPos.Add(pos + shift);
				});
				if (pos != firstRoom_coord)
					coridors.Add(coridor);
			}
		}

		// Normilising coordinates in matrix;
		Func<HashSet<Vector2Int>, List<GameObject>, Matrix<Room>>
			GetMatrix = (posses, rooms) =>
			{
				int minX = 0, minY = 0, maxX = 0, maxY = 0;
				foreach (var i in posses) {
					if (i.x < minX) minX = i.x;
					if (i.y < minY) minY = i.y;
					if (i.x > maxX) maxX = i.x;
					if (i.y > maxY) maxY = i.y;
				}
				Vector2Int shift = new(minX, minY);
				Matrix<Room> mr = new(maxX - minX + 1, maxY - minY + 1);
				foreach (var i in poses) {
					if (i == lastRoom_coord && lastRoom != null)
						mr[i - shift] = new Room(lastRoom, level, true);
					else if (i == firstRoom_coord && firstRoom != null)
						mr[i - shift] = new Room(firstRoom, level, false, true);
					else
						mr[i - shift] = new Room(rooms[random.Next((int)1e9 + 7) % rooms.Count], level);
				}
				lastRoom_coord -= shift;
				firstRoom_coord -= shift;

				foreach (var coridor_ in coridors) {
					var coridor = coridor_ - shift;
					if (coridor.y != (int)coridor.y) {
						mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Up);
						mr[(int)coridor.x, (int)coridor.y + 1].AddDoor(DoorDirection.Down);
					}
					else if (coridor.x != (int)coridor.x) {
						mr[(int)coridor.x, (int)coridor.y].AddDoor(DoorDirection.Right);
						mr[(int)coridor.x + 1, (int)coridor.y].AddDoor(DoorDirection.Left);
					}
					else throw new Exception("Coridor is incorrect " + coridor + " " + coridor_);
				}
				return mr;
			};

		// Normilizing sizes and room positions
		Func<Matrix<Room>, Matrix<Room>> NormilizeRooms = (mr) =>
		{
			int[] szw = new int[mr.w + 1];
			int[] szh = new int[mr.h + 1];
			for (int i = 0; i < mr.w; ++i)
				for (int j = 0; j < mr.h; ++j)
					if (mr[i, j] != null) {
						if (szw[i + 1] < mr[i, j].w) szw[i + 1] = mr[i, j].w;
						if (szh[j + 1] < mr[i, j].h) szh[j + 1] = mr[i, j].h;
					}

			for(int i=0;i<szw.Length-1;++i)szw[i+1]+=random.Next((int)1e+7)%5*2+5;
			for(int i=0;i<szh.Length-1;++i)szh[i+1]+=random.Next((int)1e+7)%5*2+5;

			for (int i = 0; i < szw.Length - 1; ++i) szw[i + 1] += szw[i];
			for (int i = 0; i < szh.Length - 1; ++i) szh[i + 1] += szh[i];
			for (int i = 0; i < mr.w; ++i)
				for (int j = 0; j < mr.h; ++j)
					if (mr[i, j] != null)
						mr[i, j].coord = new Vector2(szw[i + 1] * 0.5f + szw[i] * 0.5f, szh[j + 1] * 0.5f + szh[j] * 0.5f);
			return mr;
		};

		// Getting main matrix
		var mr = NormilizeRooms(GetMatrix(poses, rooms));

		level.create(mr, new(coridors.Count));

		// Generating coridors
		for (int i = 0; i < mr.w; ++i)
			for (int j = 0; j < mr.h; ++j) {
				if (mr[i, j] != null) {
					if (i > 0 && (mr[i, j].doors & (1 << (int)DoorDirection.Left)) != 0) {
						Room.GenerateHall(
							mr[i - 1, j].coord + Vector2.right * (mr[i - 1, j].w + 1) * 0.5f,
							mr[i, j].coord + Vector2.left * (mr[i, j].w - 1) * 0.5f,
							ehall, level
							);
					}
					if (j > 0 && (mr[i, j].doors & (1 << (int)DoorDirection.Down)) != 0) {
						Room.GenerateHall(
							mr[i, j - 1].coord + Vector2.up * (mr[i, j - 1].h + 1) * 0.5f,
							mr[i, j].coord + Vector2.down * (mr[i, j].h - 3) * 0.5f,
							nhall, level
							);
					}
				}
			}
		// Normilizing rooms by world coord;
		Vector2 shift = -mr[firstRoom_coord].coord;
		mr.ForEach((room) =>
		{
			if (room != null) {
				room.coord += shift;
				room.SetUp();
			}
		});
		level.halls.ForEach((hall)=>hall.transform.position+=(Vector3)shift);
		return this;
	}

  /// <summary> Расставляет лаву и лед в комнате </summary>
  /// <param name="room"> Комната <see cref="IRoomTilemapController"></param>
  /// <param name="additional"> Тип тайла <see cref="TileBase"></param>
  /// <param name="modificator"> Доля льда или лавы от свободных тайлов команты </param>
	void GenerateAdditional(IRoomTilemapController room,
      TileBase additional, float modificator = 1/16.0f) {
		var poses = room.GetFreePositions();
		float iter = poses.Count * modificator;
		while(iter-->0){
			int ind = UnityEngine.Random.Range(0, poses.Count);
			room.SetAdditionalTile(additional, poses[ind]);
			poses[ind] = poses[poses.Count-1];
			poses.RemoveAt(poses.Count-1);
		}
	}

  /// <summary> Устанавливает дополнительные ловушки в комнате </summary>
  /// <param name="room"> Комната <see cref="Room"></param>
  /// <param name="trapContainer"> Контейнер с ловушками <see cref="TrapContainer"></param>
	void SetAdditionalEffect(Room room, TrapContainer trapContainer) {
		var additionalController = room.GetRoomController().AdditionalController;
		if (additionalController == null) return;
		if (trapContainer == null) return;
		additionalController.SetAdditionalEffect(trapContainer);
	}

  /// <summary> Заменяет тайлы комнаты на соответствующие локации </summary>
  /// <param name="room"> Комната <see cref="Room"></param>
  /// <param name="tiles"> Контейнер с тайлами <see cref="TilesContainer"></param>
  /// <param name="trapContainer"> Контейнер с ловушками <see cref="TrapContainer"></param>
	void MakeTiles(Room room, TilesContainer tiles, TrapContainer trapContainer) {
		var tilemap = room.GetRoomTilemapController();
		if (tiles.Additional == null)
			tilemap.RemoveAdditional();
		else {
			GenerateAdditional(tilemap, tiles.Additional);
			SetAdditionalEffect(room, trapContainer);
		}
		tilemap.SwapTiles(tiles);
	}

  /// <summary> Функция активации комнат. Завершает генерацию уровня </summary>
  /// <returns> Generator - используется в цевочке </returns>
  /// <param name="_level"> Уровень с комнатами и коридорами <see cref="Level"></param>
  /// <param name="tiles"> Контейнер с тайлами <see cref="TilesContainer"></param>
  /// <param name="traps"> Контейнер с ловушками <see cref="TrapContainer"></param>
	public Generator ActivateLevel(Level _level, TilesContainer tiles, TrapContainer traps){
		if(!_level.created()){
			Exc("Level must be created at Activator");
			return this;
		}
		_level.rooms.ForEach((r) => {
			if (r == null) return;
			MakeTiles(r, tiles, traps);
			GameObject.Destroy(r.GetRoomTilemapController() as MonoBehaviour);
			r.GetRoomController().ActivateRoom();
		});

		_level.halls.ForEach((h) =>
			h.GetComponent<IHallTilemapController>().SwapTiles(tiles)
		);
		return this;
	}
}
