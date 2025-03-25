using System;
using UnityEngine;

/// <summary>
/// Controller for additional effects like lava and ice effects
/// Should do something with entities entered in lava or ice
/// </summary>
public class AdditionallController : MonoBehaviour
{
    //TODO: add some field for effect

    void OnTriggerEnter2D(Collider2D collision)
    {
        throw new NotImplementedException("Not impemented additional effect impact on entities");
    }
}
