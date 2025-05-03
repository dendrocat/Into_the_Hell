using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyManager : MonoBehaviour
{
    public static DontDestroyManager Instance { get; private set; }
    [SerializeField] List<GameObject> dontDestroyable;

    void Awake()
    {
        if (Instance != null) DestroyAll();
        Instance = this;
        var childs = transform.GetComponentsInChildren<Transform>();
        foreach (var item in childs)
        {
            item.transform.parent = null;
            DontDestroyOnLoad(item.gameObject);
            dontDestroyable.Add(item.gameObject);
        }
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return null;
        yield return null;
        GameManager.Instance.ToMainMenu();
    }

    public void DestroyAll()
    {
        Destroy(gameObject);
        foreach (var item in dontDestroyable)
        {
            Destroy(item.gameObject);
        }
    }
}
