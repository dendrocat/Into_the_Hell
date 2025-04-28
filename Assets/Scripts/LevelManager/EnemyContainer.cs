using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyContainer", menuName = "Scriptable Objects/EnemyContainer")]
public class EnemyContainer : ScriptableObject
{
    [SerializeField] List<GameObject> _enemies;

    [SerializeField] GameObject _boss;

    public List<GameObject> Enemies => _enemies;

    public GameObject Boss => _boss;
}
