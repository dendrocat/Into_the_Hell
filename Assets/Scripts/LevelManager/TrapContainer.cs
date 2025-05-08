using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrapContainer", menuName = "Containers/TrapContainer")]
public class TrapContainer : ScriptableObject
{
    [SerializeField] string _trapTypeName;

    [SerializeField] float _trapCheckPeriod;

    [SerializeField] List<string> _targetTags;


    public Type TrapType => Type.GetType(_trapTypeName);

    public float TrapCheckPeriod => _trapCheckPeriod;

    public List<string> TargetTags => _targetTags;
}
