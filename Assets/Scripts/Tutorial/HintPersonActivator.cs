using UnityEngine;

[RequireComponent(typeof(Person))]
public class HintPersonActivator : HintActivator
{
    protected override void ActivateHint()
    {
        GetComponent<Person>().OnHealthChanged.RemoveListener(ActivateHint);
        base.ActivateHint();
    }

    void Awake()
    {
        GetComponent<Person>().OnHealthChanged.AddListener(ActivateHint);
    }
}
