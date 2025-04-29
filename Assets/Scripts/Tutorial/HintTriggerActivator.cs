using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HintTriggerActivator : HintActivator
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ActivateHint();
    }
}
