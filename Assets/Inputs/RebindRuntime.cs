using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

public class RebindRuntime : MonoBehaviour
{
    [SerializeField] RebindActionUI rebindActionUI;

    [SerializeField] InputActionReference reference;

    void Awake()
    {
        bool flag = false;
        var actions = InputManager.Instance.GetActions();
        Debug.Log(reference.action);
        foreach (var i1 in actions.actionMaps)
        {
            foreach (var i2 in i1.actions)
            {
                Debug.Log(i2);
                if (i2 == reference.action)
                {
                    var refer = ScriptableObject.CreateInstance<InputActionReference>();
                    refer.Set(i2);
                    rebindActionUI.actionReference = refer;
                    flag = true;
                    break;
                }
            }
            if (flag) break;
        }
        rebindActionUI.UpdateBindingDisplay();
        Destroy(this);
    }
}
