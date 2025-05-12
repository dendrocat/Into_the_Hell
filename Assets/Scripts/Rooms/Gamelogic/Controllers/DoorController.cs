using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер для управления дверями в сцене. Реализует интерфейс <see cref="IDoorController"/>.
/// Отвечает за обнаружение дверей, их открытие и закрытие.
/// </summary>
public class DoorController : MonoBehaviour, IDoorController
{
    /// <summary>
    /// Список компонентов <see cref="DoorMover"/>, представляющих двери в комнате.
    /// </summary>
    List<DoorMover> _doors;

    /// <summary>
    /// Вызывается при старте игры.
    /// Запускает корутину проверки наличия дверей.
    /// </summary>
    void Start()
    {
        StartCoroutine(CheckDoors());
    }

    /// <summary>
    /// Корутина для проверки корректности списка дверей.
    /// Выводит ошибки, если список не установлен или пуст.
    /// </summary>
    /// <returns>IEnumerator для использования в корутине.</returns>
    IEnumerator CheckDoors()
    {
        yield return null;
        if (_doors == null)
        {
            Debug.LogError($"{GetType()}: The doors were not setted.");
        }
        else if (_doors.Count == 0)
        {
            Debug.LogError($"{GetType()}: In the room must be at least one door.");
        }
    }

    /// <inheritdoc />
    public void SetDoors(List<GameObject> doors)
    {
        _doors = new();
        doors.ForEach(d => _doors.Add(d.GetComponent<DoorMover>()));
    }
    /// <inheritdoc />
    public void OpenDoors()
    {
        _doors.ForEach(d => d.OpenDoor());
    }

    /// <inheritdoc />
    public void CloseDoors()
    {
        _doors.ForEach(d => d.CloseDoor());
    }

    /// <inheritdoc />
    public void RemoveDoors()
    {
        _doors.ForEach(d => Destroy(d.gameObject));
    }
}
