using Pathfinding;
using UnityEngine;

/// <summary>
/// Controller for managing rooms in the game. Implements the <see cref="IRoomController"/> interface.
/// Handles room-specific logic such as boss and enemy spawns, additional effects, and door control.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(PlayerInRoomHolder))]
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

    PlayerInRoomHolder playerHolder;

    void Awake()
    {
        DoorController = GetComponentInChildren<IDoorController>();
        AdditionalController = GetComponentInChildren<IAdditionalController>();
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        playerHolder = GetComponent<PlayerInRoomHolder>();
        playerHolder.enabled = false;
    }

    public void ActivateRoom()
    {
        _collider.enabled = true;
        if (!EnemySpawns.Enabled && !BossSpawn.Enabled)
            RoomFinished(true);
    }

    int enemiesCount = 0;

    void OnBossDied(Person boss)
    {
        if (!(boss is Boss)) return;
        Person.Died.RemoveListener(OnBossDied);
        RoomFinished();
    }

    void SpawnBoss(Transform playerTransform)
    {
        if (!BossSpawn.Enabled) return;
        var bossPrefab = EnemyStorage.Instance.GetEnemies().Boss;
        var boss = Instantiate(bossPrefab, BossSpawn.Value.position, Quaternion.identity);
        if (boss.TryGetComponent<BlazeAIController>(out var blazeAI))
        {
            blazeAI.SetAttackTarget(playerTransform);
        }
        else
            boss.GetComponent<AIDestinationSetter>().target = playerTransform;
        UIManager.Instance.SetBoss(boss.GetComponent<Boss>());
        Person.Died.AddListener(OnBossDied);

        Destroy(BossSpawn.Value.gameObject);
    }

    void SpawnEnemies(Transform playerTransform)
    {
        if (!EnemySpawns.Enabled) return;
        var enemies = EnemyStorage.Instance.GetEnemies().Enemies;

        enemiesCount = EnemySpawns.Value.childCount;
        for (int i = 0; i < enemiesCount; i++)
        {
            var obj = Instantiate(enemies[Random.Range(0, enemies.Count)],
                        EnemySpawns.Value.GetChild(i).position,
                        Quaternion.identity
                    );
            obj.GetComponent<AIDestinationSetter>().target = playerTransform;
        }
        Person.Died.AddListener(OnEnemyDied);
        Destroy(EnemySpawns.Value.gameObject);
    }

    void StartBattle(Transform playerTransform)
    {
        playerHolder.enabled = true;
        playerHolder.SetPlayer(playerTransform.gameObject.GetComponent<Player>());

        SpawnEnemies(playerTransform);
        SpawnBoss(playerTransform);

        DoorController.CloseDoors();

        var astar = FindFirstObjectByType<AstarPath>();
        (astar.graphs[0] as GridGraph).center = transform.position;
        astar.Scan();
    }

    void OnEnemyDied(Person person)
    {
        if (person is Player) return;
        --enemiesCount;
        if (enemiesCount == 0) RoomFinished();
    }

    void RoomFinished(bool force = false)
    {
        Person.Died.RemoveListener(OnEnemyDied);
        if (!force) DoorController.OpenDoors();
        else DoorController.RemoveDoors();

        Destroy(this);

        if (_isEndRoom)
            ExitManager.Instance.SpawnExit(transform.position);
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
        if (IsPlayerEntered(collision) && !playerHolder.enabled) StartBattle(collision.transform);
    }

    public void MarkAsEndRoom()
    {
        _isEndRoom = true;
    }

    void OnDestroy()
    {
        Destroy(DoorController as MonoBehaviour);
        Destroy(playerHolder);
    }
}
