using UnityEngine;

/// <summary>
/// Controller for managing additional effects like lava and ice.
/// Handles interactions with entities that enter areas affected by these effects.
/// </summary>
public class AdditionalController : MonoBehaviour, IAdditionalController
{
    //TODO: add some field for effect
    public void SetAdditionalEffect()
    {
        Debug.LogError("Setting additional effect is not implemented");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Not impemented additional effect impact on entities");
    }
}
