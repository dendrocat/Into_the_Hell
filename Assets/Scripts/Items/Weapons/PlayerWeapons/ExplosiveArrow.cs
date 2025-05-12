using UnityEngine;


/// <summary>
/// Класс, описывающий взрывную стрелу
/// </summary>
public class ExplosiveArrow : Arrow
{
    /// <summary>
    /// Префаб взрыва, который создаётся при уничтожении стрелы.
    /// </summary>
    [Tooltip("Префаб взрыва, который создаётся при уничтожении стрелы")]
    [SerializeField] GameObject explosionPrefab;

    // <summary>
    /// Урон взрыва.
    /// </summary>
    float _explosionDamage;

    /// <summary>
    /// Метод, вызываемый при уничтожении объекта.
    /// Создаёт взрыв с заданным уроном и тегами целей.
    /// </summary>
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
