using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogableNPC : MonoBehaviour, IInteractable
{
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
}
