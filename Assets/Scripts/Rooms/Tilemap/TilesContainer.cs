using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Контейнер для тайлов и префабов, используемых в конкретных локациях.
/// Этот класс является ScriptableObject и может быть создан через Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "TilesContainer", menuName = "Containers/TilesContainer")]
public class TilesContainer : ScriptableObject
{
    [Header("Стены")]

    /// <summary>
    /// Тайл для крыши.
    /// </summary>
    [Tooltip("Тайл для крыши")]
    [SerializeField] TileBase _roof;

    /// <summary>
    /// Тайл для внутренних стен комнаты.
    /// </summary>
    [Tooltip("Тайл для внутренних стен комнаты")]
    [SerializeField] TileBase _wallInner;

    /// <summary>
    /// Тайл для наружных стен комнаты.
    /// </summary>
    [Tooltip("Тайл для наружных стен комнаты")]
    [SerializeField] TileBase _wallOuter;


    [Header("Пол")]

    /// <summary>
    /// Тайл для пола.
    /// </summary>
    [Tooltip("Тайл для пола")]
    [SerializeField] TileBase _floor;


    [Header("Двери")]

    /// <summary>
    /// Тайл для крыши двери.
    /// </summary>
    [Tooltip("Тайл для крыши двери")]
    [SerializeField] TileBase _doorRoof;

    /// <summary>
    /// Тайл для стены двери.
    /// </summary>
    [Tooltip("Тайл для стены двери")]
    [SerializeField] TileBase _doorWall;


    [Header("Дополнительные объекты")]

    /// <summary>
    /// Тайл для дополнительных ловушек.
    /// </summary>
    [Tooltip("Тайл для дополнительных ловушек")]
    [SerializeField] TileBase _additional;

    /// <summary>
    /// Префаб для установки ловушек.
    /// </summary>
    [Tooltip("Префаб для установки ловушек")]
    [SerializeField] GameObject _trap;

    /// <summary>
    /// Префаб для установки разрушаемых объектов.
    /// </summary>
    [Tooltip("Префаб для установки разрушаемых объектов")]
    [SerializeField] GameObject _destroys;


    // <summary>
    /// Выдает тайл крыши.
    /// </summary>
    public TileBase Roof => _roof;

    /// <summary>
    /// Выдает тайл внутренних стен комнаты.
    /// </summary>
    public TileBase WallInner => _wallInner;

    /// <summary>
    /// Выдает тайл наружных стен комнаты.
    /// </summary>
    public TileBase WallOuter => _wallOuter;

    /// <summary>
    /// Выдает тайл пола.
    /// </summary>
    public TileBase Floor => _floor;

    /// <summary>
    /// Выдает тайл крыши двери.
    /// </summary>
    public TileBase DoorRoof => _doorRoof;

    /// <summary>
    /// Выдает тайл стены двери.
    /// </summary>
    public TileBase DoorWall => _doorWall;

    /// <summary>
    /// Выдает тайл дополнительных элементов.
    /// </summary>
    public TileBase Additional => _additional;

    /// <summary>
    /// Выдает префаб для установки ловушек.
    /// </summary>
    public GameObject Trap => _trap;

    /// <summary>
    /// Выдает префаб для установки разрушаемых объектов.
    /// </summary>
    public GameObject Destroys => _destroys;
}
