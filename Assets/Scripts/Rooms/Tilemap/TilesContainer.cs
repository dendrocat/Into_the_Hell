using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Container for tiles and prefabs used in specific locations. This is a ScriptableObject that can be created via the Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "TilesContainer", menuName = "Scriptable Objects/TilesContainer")]
public class TilesContainer : ScriptableObject
{
    [Tooltip("Tile for roof")]
    [SerializeField] TileBase _roof;

    [Tooltip("Tile for walls inside the room")]
    [SerializeField] TileBase _wallInner;

    [Tooltip("Tile for walls outside the room")]
    [SerializeField] TileBase _wallOuter;

    [Tooltip("Tile for floor")]
    [SerializeField] TileBase _floor;

    [Tooltip("Tile for holes")]
    [SerializeField] TileBase _hole;

    [Tooltip("Tile for additional")]
    [SerializeField] TileBase _additional;

    [Tooltip("Prefab for setting traps")]
    [SerializeField] GameObject _trap;

    [Tooltip("Prefab for setting destroyable objects")]
    [SerializeField] GameObject _destroys;


    /// <summary>
    /// Gets the tile for the roof.
    /// </summary>
    public TileBase Roof => _roof;

    /// <summary>
    /// Gets the tile for walls inside the room.
    /// </summary>
    public TileBase WallInner => _wallInner;

    /// <summary>
    /// Gets the tile for walls outside the room.
    /// </summary>
    public TileBase WallOuter => _wallOuter;

    /// <summary>
    /// Gets the tile for the floor.
    /// </summary>
    public TileBase Floor => _floor;

    /// <summary>
    /// Gets the tile for holes.
    /// </summary>
    public TileBase Hole => _hole;

    /// <summary>
    /// Gets the tile for additional elements.
    /// </summary>
    public TileBase Additional => _additional;

    /// <summary>
    /// Gets the prefab for setting traps.
    /// </summary>
    public GameObject Trap => _trap;

    /// <summary>
    /// Gets the prefab for setting destroyable objects.
    /// </summary>
    public GameObject Destroys => _destroys;
}
