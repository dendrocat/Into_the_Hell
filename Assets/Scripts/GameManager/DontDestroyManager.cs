using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Менеджер, обеспечивающий сохранение указанных объектов между сценами.
/// Реализует паттерн Singleton.
/// </summary>
public class DontDestroyManager : MonoBehaviour
{
    /// <summary>
    /// Статический экземпляр менеджера (Singleton).
    /// </summary>
    public static DontDestroyManager Instance { get; private set; }

    /// <summary>
    /// Список объектов, которые не должны уничтожаться при загрузке новой сцены.
    /// </summary>
    [Tooltip("Список объектов, которые не уничтожаются при загрузке новой сцены")]
    [SerializeField] List<GameObject> dontDestroyable;

    /// <summary>
    /// Инициализация Singleton и перенос дочерних объектов в корень сцены с защитой от уничтожения.
    /// </summary>
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
    }

    /// <summary>
    /// Метод вызывается после Awake, переключает игру в главное меню.
    /// </summary>
    void Start()
    {
        GameManager.Instance.ToMainMenu();
    }

    /// <summary>
    /// Уничтожает все объекты, которые не должны сохраняться, и сбрасывает Singleton.
    /// </summary>
    public void DestroyAll()
    {
        Destroy(gameObject);
        foreach (var item in dontDestroyable)
        {
            Destroy(item.gameObject);
        }
        Instance = null;
    }
}
