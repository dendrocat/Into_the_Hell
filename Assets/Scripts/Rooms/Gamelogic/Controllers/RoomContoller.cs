using Pathfinding;
using UnityEngine;

/// <summary>
/// Контроллер для управления комнатами в игре. Реализует интерфейс <see cref="IRoomController"/>.
/// Отвечает за логику комнаты: спавн босса и врагов, дополнительные эффекты и управление дверями.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(PersonInRoomHolder))]
public class RoomContoller : MonoBehaviour, IRoomController
{
    /// <summary>
    /// Опциональная ссылка на точку спавна босса.
    /// </summary>
    [Tooltip("Опциональная точка спавна босса")]
    [SerializeField] Optional<Transform> BossSpawn;

    /// <summary>
    /// Опциональная ссылка на трансформ с точками спавна врагов.
    /// </summary>
    [Tooltip("Опциональный контейнер точек спавна врагов")]
    [SerializeField] Optional<Transform> EnemySpawns;

    /// <summary>
    /// Флаг, указывающий, является ли комната конечной.
    /// </summary>
    [Tooltip("Флаг, указывающий, является ли комната конечной.")]
    [SerializeField] bool _isEndRoom;

    /// <inheritdoc />
    public IAdditionalController AdditionalController { get; private set; }

    /// <inheritdoc />
    public IDoorController DoorController { get; private set; }

    Collider2D _collider;
    PersonInRoomHolder personHolder;
    int enemiesCount = 0;

    /// <summary>
    /// Инициализация компонентов и отключение коллайдера и удержания персонажей до активации.
    /// </summary>
    void Awake()
    {
        DoorController = GetComponentInChildren<IDoorController>();
        AdditionalController = GetComponentInChildren<IAdditionalController>();
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        personHolder = GetComponent<PersonInRoomHolder>();
        personHolder.enabled = false;
    }

    /// <summary>
    /// Активирует комнату, включая коллайдер и проверяя необходимость завершения комнаты.
    /// </summary>
    public void ActivateRoom()
    {
        _collider.enabled = true;
        if (!EnemySpawns.Enabled && !BossSpawn.Enabled)
            RoomFinished(true);
    }


    /// <summary>
    /// Обработчик смерти босса. Завершает комнату.
    /// </summary>
    /// <param name="boss">Объект босса, который умер.</param>
    void OnBossDied(Person boss)
    {
        if (!(boss is Boss)) return;
        Person.Died.RemoveListener(OnBossDied);
        RoomFinished();
    }

    /// <summary>
    /// Спавнит босса в комнате и настраивает его поведение.
    /// </summary>
    /// <param name="playerTransform">Трансформ игрока для задания цели атаки.</param>
    void SpawnBoss(Transform playerTransform)
    {
        if (!BossSpawn.Enabled) return;

        var boss = Instantiate(AbstractLevelManager.Instance.GetEnemies().Boss,
                            BossSpawn.Value.position, Quaternion.identity);
        personHolder.AddPerson(boss.GetComponent<Person>());

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

    /// <summary>
    /// Спавнит врагов в комнате и настраивает их поведение.
    /// </summary>
    /// <param name="playerTransform">Трансформ игрока для задания цели атаки.</param>
    void SpawnEnemies(Transform playerTransform)
    {
        if (!EnemySpawns.Enabled) return;
        var enemies = AbstractLevelManager.Instance.GetEnemies().Enemies;

        enemiesCount = EnemySpawns.Value.childCount;
        for (int i = 0; i < enemiesCount; i++)
        {
            var obj = Instantiate(enemies[Random.Range(0, enemies.Count)],
                        EnemySpawns.Value.GetChild(i).position,
                        Quaternion.identity
                    );
            obj.GetComponent<AIDestinationSetter>().target = playerTransform;
            personHolder.AddPerson(obj.GetComponent<Person>());
        }
        Person.Died.AddListener(OnEnemyDied);
        Destroy(EnemySpawns.Value.gameObject);
    }

    /// <summary>
    /// Запускает бой в комнате: включает удержание персонажей, спавнит врагов и босса, закрывает двери и обновляет навигацию.
    /// </summary>
    /// <param name="playerTransform">Трансформ игрока.</param>
    void StartBattle(Transform playerTransform)
    {
        personHolder.enabled = true;
        personHolder.AddPerson(playerTransform.gameObject.GetComponent<Player>());

        SpawnEnemies(playerTransform);
        SpawnBoss(playerTransform);

        DoorController.CloseDoors();

        var astar = FindFirstObjectByType<AstarPath>();
        (astar.graphs[0] as GridGraph).center = transform.position;
        astar.Scan();
    }

    /// <summary>
    /// Обработчик смерти врага. Уменьшает счётчик врагов и завершает комнату при их отсутствии.
    /// </summary>
    /// <param name="person">Объект персонажа, который умер.</param>
    void OnEnemyDied(Person person)
    {
        if (person is Player) return;
        --enemiesCount;
        if (enemiesCount == 0) RoomFinished();
    }

    /// <summary>
    /// Завершает комнату: открывает или удаляет двери, уничтожает контроллер комнаты и создаёт выход, если это конечная комната.
    /// </summary>
    /// <param name="force">Если <see langword="true"/>, сразу удаляет двери без открытия.</param>
    void RoomFinished(bool force = false)
    {
        Person.Died.RemoveListener(OnEnemyDied);
        if (!force) DoorController.OpenDoors();
        else DoorController.RemoveDoors();

        Destroy(this);

        if (_isEndRoom)
            ExitManager.Instance.SpawnExit(transform.position);
    }

    /// <summary>
    /// Проверяет, полностью ли игрок вошёл в комнату.
    /// </summary>
    /// <param name="player">Коллайдер игрока.</param>
    /// <returns>True, если игрок полностью внутри комнаты.</returns>
    bool IsPlayerEntered(Collider2D player)
    {
        Bounds containerBounds = _collider.bounds;
        Bounds targetBounds = player.bounds;

        return containerBounds.Contains(targetBounds.min) &&
               containerBounds.Contains(targetBounds.max);
    }

    /// <summary>
    /// Вызывается при нахождении объекта в триггере комнаты.
    /// Запускает бой, если игрок вошёл в комнату и бой ещё не начался.
    /// </summary>
    /// <param name="collision">Коллайдер объекта, находящегося в триггере.</param>
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (IsPlayerEntered(collision) && !personHolder.enabled) StartBattle(collision.transform);
    }

    /// <inheritdoc />
    public void MarkAsEndRoom()
    {
        _isEndRoom = true;
    }

    /// <summary>
    /// Очистка ресурсов при уничтожении объекта.
    /// </summary>
    void OnDestroy()
    {
        Destroy(DoorController as MonoBehaviour);
        Destroy(personHolder);
    }
}
