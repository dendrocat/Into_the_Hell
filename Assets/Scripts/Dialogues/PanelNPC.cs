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
        InputManager.Instance.PushInputMap(InputMap.UI);
        panel.SetActive(true);
        StartCoroutine(DeactivatePanel());
    }

    IEnumerator DeactivatePanel()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        InputManager.Instance.PopInputMap();
        panel.SetActive(false);
    }
}
