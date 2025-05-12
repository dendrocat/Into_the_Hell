/// <summary>
/// Интерфейс, используемый для обработки получения урона объектом.
/// </summary>
///
public interface IDamagable
{
    /// <summary>
    /// Метод, обрабатывающий получение урона объектом.
    /// </summary>
    /// <param name="damage">Количество получаемого урона</param>
    /// <param name="type"><see cref="DamageType">Тип урона</see>, наносимого объекту</param>
    public void TakeDamage(float damage, DamageType type);
}
