using System;
using UnityEngine;

/// <summary>
/// Controller for managing rooms in the game. Implements the <see cref="IRoomController"/> interface.
/// Handles room-specific logic such as boss and enemy spawns, additional effects, and door control.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RoomContoller : MonoBehaviour, IRoomController
{
    /// <summary>
    /// Optional reference to the boss spawn point.
    /// </summary>
    [SerializeField] Optional<Transform> BossSpawn;

    /// <summary>
    /// Optional reference to the transform containing spawn points for enemies.
    /// </summary>
    [SerializeField] Optional<Transform> EnemySpawns;

    /// <inheritdoc />
    public IAdditionalController AdditionalController { get; private set; }

    /// <inheritdoc />
    public IDoorController DoorController { get; private set; }

    void Awake()
    {
        DoorController = GetComponentInChildren<IDoorController>();
        AdditionalController = GetComponentInChildren<IAdditionalController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Logic for entering in room not implementing");
    }
}
