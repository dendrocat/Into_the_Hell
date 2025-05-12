using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за спавн мини-големов в зависимости от здоровья босса.
/// </summary>
[RequireComponent(typeof(Boss))]
public class MiniGolemSpawner : MonoBehaviour
{
    /// <summary>
    /// Ссылка на босса, для которого происходит спавн мини-големов.
    /// </summary>
    Boss boss;

    /// <summary>
    /// Данные спавна: количество големов и порог здоровья босса.
    /// </summary>
    List<SpawnData> spawnData = new();

    /// <summary>
    /// Префаб мини-голема для инстанциирования.
    /// </summary>
    [Tooltip("Префаб мини-голема для спавна")]
    [SerializeField] GameObject miniGolemPrefab;

    // <summary>
    /// Инициализация: получение ссылки на босса и настройка данных спавна.
    /// </summary>
    private void Start()
    {
        boss = GetComponent<Boss>();

        spawnData.Add(new SpawnData(4, 0.75f));
        spawnData.Add(new SpawnData(5, 0.5f));
        spawnData.Add(new SpawnData(6, 0.25f));
    }

    /// <summary>
    /// Проверка здоровья босса и спавн мини-големов при достижении порогов.
    /// </summary>
    private void Update()
    {
        float bossHealth = boss.getHP();
        foreach (SpawnData data in spawnData)
        {
            if (data.passed) continue;
            if (bossHealth / boss.MaxHealth < data.healthPercentage)
            {
                data.passed = true;
                Debug.Log("Barrier passed (" + data.healthPercentage * 100 + "%)");
                SpawnMiniGolems(data.count);
            }
        }
    }

    /// <summary>
    /// Спавн заданного количества мини-големов вокруг босса.
    /// </summary>
    /// <param name="count">Количество мини-големов для спавна.</param>
    void SpawnMiniGolems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 position = transform.position + Quaternion.Euler(0f, 0f, 360.0f * ((float)i / count)) * Vector2.right * 1.5f;
            GameObject miniGolem = Instantiate(miniGolemPrefab, position, Quaternion.Euler(0f, 0f, 0f));
            boss.AddEffect(EffectNames.MiniGolem, 1);
            miniGolem.GetComponent<AIDestinationSetter>().target = GetComponent<AIDestinationSetter>().target;
            miniGolem.GetComponent<MiniGolem>().golemBoss = boss;
        }
    }
}
