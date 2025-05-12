using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Компонент NPC, с которым можно взаимодействовать для запуска диалога.
/// </summary>
/// <remarks>
/// Требует компонент <see cref="Collider2D"/> для обнаружения игрока в зоне взаимодействия.
/// При входе игрока в триггер отображается подсказка, при выходе - скрывается.
/// При взаимодействии запускает диалог с использованием <see cref="DialogManager"/>.
/// </remarks>
[RequireComponent(typeof(Collider2D))]
public class DialogableNPC : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Событие, вызываемое при начале диалога.
    /// </summary>
    [HideInInspector] public UnityEvent DialogStarted = new();

    [Tooltip("Префаб изображения подсказки взаимодействия")]
    [SerializeField] GameObject _interactImage;

    GameObject _hint;
    
    [Tooltip("JSON-файл с историей Ink для диалога")]
    [SerializeField] TextAsset inkJSONFile;

    /// <summary>
    /// Инициализация компонента. Создаёт подсказку взаимодействия и проверяет наличие дубликатов компонента.
    /// </summary>
    void Awake()
    {
        if (GetComponents<DialogableNPC>().Length > 1)
        {
            Debug.LogError("Many DialogableNPC");
            return;
        }
        _hint = Instantiate(_interactImage, transform);
        _hint.transform.position += new Vector3(.5f, 1f, 0);
        _hint.SetActive(false);
    }

    /// <summary>
    /// Метод взаимодействия с NPC. Запускает диалог.
    /// </summary>
    public void Interact()
    {
        SetStory();
        DialogManager.Instance.StartStory();
        DialogStarted.Invoke();
    }

    /// <summary>
    /// Устанавливает историю диалога в <see cref="DialogManager"/>.
    /// Можно переопределить для кастомного поведения.
    /// </summary>
    protected virtual void SetStory()
    {
        DialogManager.Instance.SetStory(inkJSONFile);
    }

    /// <summary>
    /// Обработчик события входа в триггер.
    /// Показывает подсказку взаимодействия, если объект - игрок.
    /// </summary>
    /// <param name="collision">Коллайдер вошедшего объекта.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        _hint.SetActive(true);
    }

    /// <summary>
    /// Обработчик события выхода из триггера.
    /// Скрывает подсказку взаимодействия, если объект - игрок.
    /// </summary>
    /// <param name="collision">Коллайдер вышедшего объекта.</param>
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        _hint.SetActive(false);
    }
}
