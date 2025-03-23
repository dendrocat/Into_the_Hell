using UnityEngine;

public enum DamageType
{
    None = 0,
    Fire = 1,
    Explosion = 2
};
// Интерфейс IDamagable для реализации получения урона объектом
public interface IDamagable
{
    public abstract void TakeDamage(float damage, DamageType type);
}
