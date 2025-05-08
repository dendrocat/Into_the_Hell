using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий взрывную стрелу
 * </summary>
 * **/
public class ExplosiveArrow : Arrow
{
    [SerializeField] GameObject explosionPrefab;

    void OnDestroy()
    {
        Debug.Log("Explosive Damage");
        var obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>();
        obj.SetDamage(damage);
        obj.SetTargetTags(targetTags);
    }
}
