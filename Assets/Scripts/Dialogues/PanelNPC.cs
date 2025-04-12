using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class PanelNPC : DialogableNPC
{
    [SerializeField] string inkFuncName;

    [SerializeField] GameObject panel;


    protected override void SetStory()
    {
        base.SetStory();
        DialogManager.Instance.BindFunction(inkFuncName, ActivatePanel);
    }


    void ActivatePanel()
    {
        InputManager.Instance.SwitchInputMap(InputMap.UI);
        panel.SetActive(true);
    }
}
