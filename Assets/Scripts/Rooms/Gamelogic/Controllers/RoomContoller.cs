using Pathfinding;
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

    [SerializeField] bool _isEndRoom;

    /// <inheritdoc />
    public IAdditionalController AdditionalController { get; private set; }

    /// <inheritdoc />
    public IDoorController DoorController { get; private set; }

    Collider2D _collider;

    bool _enemySpawned;
    void Awake()
    {
        DoorController = GetComponentInChildren<IDoorController>();
        AdditionalController = GetComponentInChildren<IAdditionalController>();
        _collider = GetComponent<Collider2D>();
    }

    int enemiesCount = 0;

    void SpawnEnemies(Transform playerTransform)
    {
        _enemySpawned = true;
        Debug.Log("SpawnEnemies");
        var enemies = EnemyStorage.Instance.GetEnemies(GameManager.Instance.Location).Enemies;

        enemiesCount = EnemySpawns.Value.childCount;
        for (int i = 0; i < enemiesCount; i++)
        {
            var obj = Instantiate(enemies[Random.Range(0, enemies.Count - 1)],
                        EnemySpawns.Value.GetChild(i).position,
                        Quaternion.identity
                    );
            obj.GetComponent<BaseEnemy>().OnDied.AddListener(OnEnemyDied);
            obj.GetComponent<AIDestinationSetter>().target = playerTransform;
        }
        Destroy(EnemySpawns.Value.gameObject);

        DoorController.CloseDoors();
        _enemySpawned = true;
    }

    void OnEnemyDied(Person enemy)
    {
        --enemiesCount;
        enemy.OnDied.RemoveListener(OnEnemyDied);
        if (enemiesCount == 0) RoomFinished();
    }

    void RoomFinished()
    {
        Debug.Log("Room Finished");
        DoorController.OpenDoors();

        Destroy(DoorController as MonoBehaviour);
        Destroy(this);
    }

    bool IsPlayerEntered(Collider2D player)
    {
        Bounds containerBounds = _collider.bounds;
        Bounds targetBounds = player.bounds;

        return containerBounds.Contains(targetBounds.min) &&
               containerBounds.Contains(targetBounds.max);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!EnemySpawns.Enabled && !BossSpawn.Enabled)
        {
            Destroy(this);
            return;
        }
        if (IsPlayerEntered(collision) && !_enemySpawned) SpawnEnemies(collision.transform);
    }

    public void MarkAsEndRoom()
    {
        _isEndRoom = true;
    }

    void OnDestroy()
    {
        if (_isEndRoom)
            Debug.Log("Exit Spawned");
    }
}
