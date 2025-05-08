using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogableNPC : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _interactImage;
    GameObject _hint;
    [SerializeField] TextAsset inkJSONFile;

    void Awake()
    {
        if (GetComponents<DialogableNPC>().Length > 1)
        {
            Debug.LogError("Many DialogableNPC");
        }
    }

    public void Interact()
    {
        SetStory();
        DialogManager.Instance.StartStory();
    }

    protected virtual void SetStory()
    {
        DialogManager.Instance.SetStory(inkJSONFile);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        _hint = Instantiate(_interactImage, transform);
        _hint.transform.position += new Vector3(.5f, 1f, 0);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;
        Destroy(_hint);
    }
}
