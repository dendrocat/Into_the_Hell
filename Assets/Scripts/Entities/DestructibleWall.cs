using UnityEngine;

/**
 * <summary>
 * �����, ����������� ����������� �����
 * </summary>
 * **/
public class DestructibleWall : MonoBehaviour, IDamagable
{
    /**
     * <inheritdoc/>
     * **/
    public void TakeDamage(float damage, DamageType type)
    {
        Bounds objectBound = GetComponent<Collider2D>().bounds;
        AstarPath.active.UpdateGraphs(objectBound);

        Destroy(gameObject);
    }
}
