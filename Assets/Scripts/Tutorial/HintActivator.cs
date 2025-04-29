using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActionRuntimeFinder))]
public abstract class HintActivator : MonoBehaviour
{
    [SerializeField] Hint _hint;

    [SerializeField] float _delayActivation = .5f;

    ActionRuntimeFinder _finder;


    void Start()
    {
        _finder = GetComponent<ActionRuntimeFinder>();
        _hint.InitHint(_finder);
    }

    IEnumerator DelayActivation()
    {
        yield return new WaitForSeconds(_delayActivation);
        HintManager.Instance.ShowHints(_hint.Hints);
        Destroy(this);
        Destroy(_finder);
    }

    protected virtual void ActivateHint()
    {
        StartCoroutine(DelayActivation());
    }
}
