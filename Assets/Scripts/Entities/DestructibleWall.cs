using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage, DamageType type)
    {
        Bounds objectBound = GetComponent<Collider2D>().bounds;
        AstarPath.active.UpdateGraphs(objectBound);

        Destroy(gameObject);
    }
}
