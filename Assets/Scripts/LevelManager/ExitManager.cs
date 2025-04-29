using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public static ExitManager Instance { get; private set; }

    [SerializeField] GameObject _exit;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnExit(Vector3 exitPosition)
    {
        Instantiate(_exit, exitPosition, Quaternion.identity);
    }
}
