using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    protected PlayerInventory _inventory;

    void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    void OnDisable()
    {
        _inventory = null;
    }


    public void Open()
    {
        InputManager.Instance.PushInputMap(InputMap.UI);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        InputManager.Instance.PopInputMap();
        gameObject.SetActive(false);
    }
}
