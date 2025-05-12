using UnityEngine;

/// <summary>
/// Контроллер для управления дополнительными эффектами, такими как лава и лёд.
/// Обрабатывает взаимодействия с сущностями, входящими в зоны действия этих эффектов.
/// </summary>
public class AdditionalController : MonoBehaviour, IAdditionalController
{
    /// <inheritdoc />
    public void SetAdditionalEffect(TrapContainer trapContainer)
    {
        var trap = gameObject.AddComponent(trapContainer.TrapType);
        (trap as BaseTrap).SetTargetTags(trapContainer.TargetTags);
        var trapController = gameObject.AddComponent<StaticTrapController>();
        trapController.SetPeriodicalCheckPeriod(trapContainer.TrapCheckPeriod);
        Destroy(this);
    }
}
