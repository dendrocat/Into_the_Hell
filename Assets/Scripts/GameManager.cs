using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Locations _location;
    public Locations Location => _location;

    [Range(1, 5)]
    [SerializeField] int level;

    void LoadData()
    {

    }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        LoadData();
    }

    void Start()
    {
        LevelManager.Instance.Generate(_location, level);
    }
}
