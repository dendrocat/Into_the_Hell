using System;
using UnityEngine;

/// <summary>
/// Controller for holes in tilemap.
/// Should do something with entities entered in holes
/// </summary>
public class HolesController : MonoBehaviour
{
    [SerializeField] HoleScriptable _hole;
    void OnTriggerEnter2D(Collider2D collision)
    {
        throw new NotImplementedException("Not impemented holes impact on entities");
    }
}
