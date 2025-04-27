using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controller for managing additional effects like lava and ice.
/// Handles interactions with entities that enter areas affected by these effects.
/// </summary>
public class AdditionalController : MonoBehaviour, IAdditionalController
{
    //TODO: add some field for effect
    public void SetAdditionalEffect(TrapContainer trapContainer)
    {
        var trap = gameObject.AddComponent(trapContainer.TrapType);
        (trap as BaseTrap).SetTargetTags(trapContainer.TargetTags);
        var trapController = gameObject.AddComponent<StaticTrapController>();
        trapController.SetPeriodicalCheckPeriod(trapContainer.TrapCheckPeriod);
        Destroy(this);
    }
}
