using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "HintConfig", menuName = "Hint")]
public class Hint : ScriptableObject
{
    [Serializable]
    struct ActionHint
    {
        public InputActionReference inputAction;
        public string hint;
    }
    [SerializeField] List<ActionHint> _actionHints;

    public List<string> Hints { get; private set; }


    string FillHintByBindings(ActionHint actionHint)
    {
        List<string> bindDisplay = new();
        var action = actionHint.inputAction.action;
        for (int bindingIndex = 0; bindingIndex < action.bindings.Count; bindingIndex++)
        {
            if (action.bindings[bindingIndex].isPartOfComposite) continue;

            bindDisplay.Add(action.GetBindingDisplayString(
                bindingIndex,
                InputBinding.DisplayStringOptions.DontIncludeInteractions
            ));
        }

        var hint = actionHint.hint;
        int cnt = hint.CountIndices('$');
        for (int i = 0; i < cnt; ++i)
            hint = hint.Replace($"${i}", bindDisplay[i]);
        return hint;
    }

    public void InitHint(ActionRuntimeFinder finder)
    {
        Hints = new();
        for (int i = 0; i < _actionHints.Count; ++i)
        {
            if (_actionHints[i].inputAction == null)
            {
                Hints.Add(_actionHints[i].hint);
                continue;
            }
            var a = new ActionHint
            {
                inputAction = finder.FindRuntimeAction(_actionHints[i].inputAction),
                hint = _actionHints[i].hint
            };
            Hints.Add(FillHintByBindings(a));
        }
    }
}
