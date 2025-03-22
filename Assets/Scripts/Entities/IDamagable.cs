using UnityEngine;

// Интерфейс IDamagable для реализации получения урона объектом
public interface IDamagable
{
    public abstract void TakeDamage(float damage);
}
