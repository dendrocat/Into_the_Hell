using UnityEngine;

/**
 * <summary>
 * Класс, описывающий взрывную стрелу
 * </summary>
 * **/
public class ExplosiveArrow : Arrow
{
    [SerializeField] GameObject explosionPrefab;

    float _explosionDamage;

    void OnDestroy()
    {
        Debug.Log("Explosive Damage");
        var obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>();
        obj.SetDamage(_explosionDamage);
        obj.SetTargetTags(targetTags);
    }

    public void SetExplosionDamage(float explosionDamage) {
        _explosionDamage = explosionDamage;
    }
}
