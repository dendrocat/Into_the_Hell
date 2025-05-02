using UnityEngine;

[RequireComponent(typeof(Person))]
public class HintPersonActivator : HintActivator
{
    protected override void ActivateHint()
    {
        GetComponent<Person>().HealthChanged.RemoveListener(ActivateHint);
        base.ActivateHint();
    }

    void Awake()
    {
        GetComponent<Person>().HealthChanged.AddListener(ActivateHint);
    }
}
