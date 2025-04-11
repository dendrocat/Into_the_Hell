using Ink.UnityIntegration;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogableNPC : MonoBehaviour, IInteractable
{
    [SerializeField] TextAsset inkJSONFile;
    public void Interact()
    {
        DialogManager.Instance.StartStory(inkJSONFile);
    }
}
