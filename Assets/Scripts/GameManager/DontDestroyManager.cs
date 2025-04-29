using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyManager : MonoBehaviour
{
    public static DontDestroyManager Instance { get; private set; }
    [SerializeField] List<GameObject> dontDestroyable;

    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        var childs = transform.GetComponentsInChildren<Transform>();
        foreach (var item in childs)
        {
            item.transform.parent = null;
            DontDestroyOnLoad(item.gameObject);
            dontDestroyable.Add(item.gameObject);
        }
    }

    void Start()
    {
        GameManager.Instance.ToMainMenu();
    }

    void OnDestroy()
    {
        foreach (var item in dontDestroyable)
        {
            Destroy(item.gameObject);
        }
    }
}
