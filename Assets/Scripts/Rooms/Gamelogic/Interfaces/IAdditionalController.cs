// <summary>
/// Интерфейс для контроллеров, управляющих дополнительными ловушками.
/// </summary>
public interface IAdditionalController
{
    /// <summary>
    /// Применяет или устанавливает дополнительные ловушки, определённые в реализующем классе.
    /// </summary>
    /// <param name="trapContainer"><see cref="TrapContainer">Контейнер ловушек</see>, содержащий данные для применения эффектов.</param>
    public void SetAdditionalEffect(TrapContainer trapContainer);
}