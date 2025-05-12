using System.Collections.Generic;
using UnityEngine;

public class Level {
  /// <summary> Transform комнаты <see cref="Transform"></summary>
	public Transform transform;
  /// <summary> Объекты коридоров <see cref="GameObject"></summary>
	public List<GameObject> halls = null;
  /// <summary> Матрица комнат на уровне <see cref="Matrix"></summary>
	public Matrix<Room> rooms = null;

  /// <summary> Конструктор </summary>
  /// <param name="transform"> Transform уровня <see cref="Transform"></param>
	public Level(Transform transform) {
		this.transform = transform;
	}

  /// <summary> Возвращает true, если уровень уже создан </summary>
  /// <returns> Создан ли уровень </returns> 
	public bool created() {
		return halls != null && rooms != null;
	}

  /// <summary> Создает уровень </summary>
  /// <param name="rooms"> Матрица комнат <see cref="Room"></param>
  /// <param name="halls"> Список коридоров <see cref="GameObject"></param>
	public void create(Matrix<Room> rooms, List<GameObject> halls) {
		this.rooms = rooms;
		this.halls = halls;
	}
}
