using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SpawnData
{
    [SerializeField] public readonly int count;
    [SerializeField] public readonly float healthPercentage;
    [SerializeField] public bool passed;

    public SpawnData(int count, float healthPercentage)
    {
        this.count = count;
        this.healthPercentage = healthPercentage;
        passed = false;
    }
}

public class MiniGolemSpawner : MonoBehaviour
{
    Boss boss;
    List<SpawnData> spawnData = new();
    [SerializeField] GameObject miniGolemPrefab;
    float bossMaxHealth;

    private void Start()
    {
        boss = GetComponent<Boss>();
        bossMaxHealth = boss.GetMaxHealth();

        spawnData.Add(new SpawnData(4, 0.75f));
        spawnData.Add(new SpawnData(5, 0.5f));
        spawnData.Add(new SpawnData(6, 0.25f));
    }

    private void Update()
    {
        float bossHealth = boss.getHP();
        foreach (SpawnData data in spawnData)
        {
            if (data.passed) continue;
            if (bossHealth / bossMaxHealth < data.healthPercentage)
            {
                data.passed = true;
                Debug.Log("Barrier passed (" + data.healthPercentage * 100 + "%)");
                SpawnMiniGolems(data.count);
            }
        }
    }

    void SpawnMiniGolems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 position = transform.position + Quaternion.Euler(0f, 0f, 360.0f * ((float) i / count)) * Vector2.right * 1.5f;
            GameObject miniGolem = Instantiate(miniGolemPrefab, position, Quaternion.Euler(0f, 0f, 0f));
            boss.AddEffect(EffectNames.MiniGolem, 1);
            miniGolem.GetComponent<AIDestinationSetter>().target = GetComponent<AIDestinationSetter>().target;
            miniGolem.GetComponent<MiniGolem>().golemBoss = boss;
        }
    }
}
