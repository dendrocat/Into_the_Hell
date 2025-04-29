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

    Player _player;

    bool _enemySpawned;
    void Awake()
    {
        DoorController = GetComponentInChildren<IDoorController>();
        AdditionalController = GetComponentInChildren<IAdditionalController>();
        _collider = GetComponent<Collider2D>();
    }

    int enemiesCount = 0;

    void OnBossDied(Person boss)
    {
        boss.OnDied.RemoveListener(OnBossDied);
        RoomFinished();
    }

    void SpawnBoss(Transform playerTransform)
    {
        if (!BossSpawn.Enabled) return;
        Debug.Log("SpawnBoss");
        var bossPrefab = EnemyStorage.Instance.GetEnemies().Boss;
        var boss = Instantiate(bossPrefab, BossSpawn.Value.position, Quaternion.identity);
        if (boss.TryGetComponent<BlazeAIController>(out var blazeAI))
        {
            blazeAI.SetAttackTarget(playerTransform);
        }
        else
            boss.GetComponent<AIDestinationSetter>().target = playerTransform;
        var bossComp = boss.GetComponent<Boss>();
        UIManager.Instance.SetBoss(bossComp);
        bossComp.OnDied.AddListener(OnBossDied);

        Destroy(BossSpawn.Value.gameObject);
    }

    void SpawnEnemies(Transform playerTransform)
    {
        if (!EnemySpawns.Enabled) return;
        Debug.Log("SpawnEnemies");
        var enemies = EnemyStorage.Instance.GetEnemies().Enemies;

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
    }

    void PlayerDied(Person player)
    {
        GameManager.ReloadGame();
    }

    void StartBattle(Transform playerTransform)
    {
        _enemySpawned = true;
        _player = playerTransform.gameObject.GetComponent<Player>();
        _player.OnDied.AddListener(PlayerDied);
        SpawnEnemies(playerTransform);
        SpawnBoss(playerTransform);

        DoorController.CloseDoors();

        var astar = FindFirstObjectByType<AstarPath>();
        (astar.graphs[0] as GridGraph).center = transform.position;
        astar.Scan();
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
        _player?.OnDied.RemoveListener(PlayerDied);


        if (_isEndRoom)
            LevelManager.Instance.SpawnExit(transform.position);
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
            RoomFinished();
            return;
        }
        if (IsPlayerEntered(collision) && !_enemySpawned) StartBattle(collision.transform);
    }

    public void MarkAsEndRoom()
    {
        _isEndRoom = true;
    }

    void FixPlayerIn()
    {
        Bounds containerBounds = _collider.bounds;
        Bounds targetBounds = _player.GetComponent<Collider2D>().bounds;

        Vector3 dt = Vector3.right + Vector3.up * 2;
        containerBounds.min -= dt - Vector3.up;
        containerBounds.max += dt;

        if (containerBounds.Contains(targetBounds.min) &&
               containerBounds.Contains(targetBounds.max)) return;

        var minBound = containerBounds.min - targetBounds.min;
        var maxBound = containerBounds.max - targetBounds.max;

        dt = Vector3.zero;
        for (int i = 0; i < 3; ++i)
        {
            if (minBound[i] > 0)
                dt[i] += minBound[i] + 1f;
            if (maxBound[i] < 0)
                dt[i] += maxBound[i] - 1f;
        }
        _player.transform.position += dt;
    }

    void FixedUpdate()
    {
        if (_player)
            FixPlayerIn();
    }
}
