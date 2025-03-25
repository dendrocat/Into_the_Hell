using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [SerializeField] bool showing;

    [SerializeField] Vector2Int size;

    void OnDrawGizmos()
    {
        if (!showing) return;
        Gizmos.color = new Color(1f, 0.5f, 0.1f, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(size.x, size.y, 0));
    }

    void Start()
    {
        Destroy(this);
    }
}
