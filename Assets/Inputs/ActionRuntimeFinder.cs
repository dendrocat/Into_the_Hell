using UnityEngine;
using UnityEngine.InputSystem;

public class ActionRuntimeFinder : MonoBehaviour
{
    public InputActionReference FindRuntimeAction(InputActionReference reference)
    {
        InputActionReference runtimeReference = null;
        var actions = InputManager.Instance.GetActions();
        foreach (var i1 in actions.actionMaps)
        {
            foreach (var i2 in i1.actions)
            {
                if (i2 == reference.action)
                {
                    runtimeReference = ScriptableObject.CreateInstance<InputActionReference>();
                    runtimeReference.Set(i2);
                    break;
                }
            }
            if (runtimeReference) break;
        }
        if (runtimeReference == null)
        {
            Debug.LogError("Action don't find in runtime input action map");
        }
        return runtimeReference;
    }
}
