using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TilesContainer", menuName = "Scriptable Objects/TilesContainer")]
public class TilesContainer : ScriptableObject
{
    [SerializeField] TileBase _wallUp;
    [SerializeField] TileBase _wallDownInner;
    [SerializeField] TileBase _wallDownOuter;
    [SerializeField] TileBase _floor;
    [SerializeField] TileBase _hole;
    [SerializeField] TileBase _additional;
    [SerializeField] GameObject _trap;
    [SerializeField] GameObject _destroys;


    public TileBase WallUp => _wallUp;
    public TileBase WallDownInner => _wallDownInner;
    public TileBase WallDownOuter => _wallDownOuter;
    public TileBase Floor => _floor;
    public TileBase Hole => _hole;
    public TileBase Additional => _additional;
    public GameObject Trap => _trap;
    public GameObject Destroys => _destroys;
}
