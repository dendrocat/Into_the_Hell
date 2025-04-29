using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStorage : MonoBehaviour
{
    public static EnemyStorage Instance { get; private set; }

    [Serializable]
    struct LocationEnemy
    {
        public Locations location;
        public EnemyContainer enemyContainer;
    }

    [SerializeField] List<LocationEnemy> locationEnemies;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public EnemyContainer GetEnemies()
    {
        var location = GameStorage.Instance.location;
        return locationEnemies[(int)location].enemyContainer;
    }
}
