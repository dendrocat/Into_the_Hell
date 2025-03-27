using System;
using UnityEngine;

/// <summary>
/// Controller for managing holes in the tilemap.
/// Handles interactions with entities that fall into holes.
/// </summary>
public class HolesController : MonoBehaviour
{
    /// <summary>
    /// Scriptable object containing data about the hole (e.g., damage, effects).
    /// </summary>
    [SerializeField] HoleScriptable _hole;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Not impemented holes impact on entities");
    }
}
