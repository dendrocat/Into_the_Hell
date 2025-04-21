using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNPC : DialogableNPC
{
    [Serializable]
    struct FuncPanel
    {
        public string inkFuncName;
        public BasePanel panel;

    }

    [SerializeField] List<FuncPanel> _funcPanels;



    protected override void SetStory()
    {
        base.SetStory();
        foreach (var panel in _funcPanels)
            DialogManager.Instance.BindFunction(
                panel.inkFuncName,
                () => panel.panel.Open()
            );
    }
}
