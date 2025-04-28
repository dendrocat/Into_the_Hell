using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

[RequireComponent(typeof(RebindActionUI))]
public class RebindRuntime : MonoBehaviour
{
    void Awake()
    {
        var rebindActionUI = GetComponent<RebindActionUI>();
        var reference = rebindActionUI.actionReference;
        bool flag = false;
        var actions = InputManager.Instance.GetActions();
        foreach (var i1 in actions.actionMaps)
        {
            foreach (var i2 in i1.actions)
            {
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
