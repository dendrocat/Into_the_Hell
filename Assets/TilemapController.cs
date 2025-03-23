using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TilemapController : MonoBehaviour
{
    [Header("For sizing")]
    [SerializeField] bool showGizmos;

    [SerializeField] Vector2 size;

    [Header("For tiles swapping")]
    [SerializeField] protected TilesContainer _templateContainer;


    public abstract void SwapTiles(TilesContainer container);


    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        Gizmos.DrawCube(transform.position, size);
    }
}
