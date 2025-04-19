using System;
using UnityEngine;

public interface IDialogManager
{
    public void SetStory(TextAsset inkJSONfile);
    public void BindFunction(string inkFuncName, Action innerFunc);

    public void StartStory();
}