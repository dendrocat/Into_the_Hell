using UnityEngine;

/// <summary>
/// Активатор подсказок, который запускает показ при входе игрока в триггер.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HintTriggerActivator : HintActivator
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ActivateHint();
    }
}
