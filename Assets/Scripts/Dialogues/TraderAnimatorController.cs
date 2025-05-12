using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер анимаций торговца, переключающийся между несколькими состояниями idle и реагирующий на начало диалога.
/// </summary>
[RequireComponent(typeof(Animator), typeof(PanelNPC))]
public class TraderAnimatorController : MonoBehaviour
{
    /// <summary>Компонет аниматор</summary>
    Animator _anim;

    /// <summary>
    /// Количество дополнительных состояний Idle (например, Idle0, Idle1, ...)
    /// </summary>
    [Tooltip("Количество дополнительных состояний Idle")]
    [SerializeField, Range(1f, 4f)] int countAdditionalIdles;

    /// <summary>
    /// Задержка перед переключением на следующее состояние idle (в секундах)
    /// </summary>
    [Tooltip("Задержка перед переключением на следующее состояние idle (в секундах)")]
    [SerializeField, Range(2f, 5f)] float delayNextIdle;

    /// <summary>
    /// Номер текущий дополнительного состояния Idle
    /// </summary>
    int currentIdle;

    /// <summary>
    /// <see cref="DialogableNPC">NPC</see> для отслеживания начала диалога
    /// </summary>
    DialogableNPC npc;

    /// <summary>
    /// Инициализация: получение компонентов, запуск корутины смены idle-анимаций и подписка на событие начала диалога.
    /// </summary>
    void Awake()
    {
        _anim = GetComponent<Animator>();
        currentIdle = 0;
        StartCoroutine(ChangingIdles());

        npc = GetComponent<PanelNPC>();
        npc.DialogStarted.AddListener(ActivateDialoging);
    }

    /// <summary>
    /// Корутина, циклично переключающая состояния Idle с задержками.
    /// </summary>
    IEnumerator ChangingIdles()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayNextIdle);
            yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
            _anim.Play($"Idle{currentIdle}");
            currentIdle = (currentIdle + 1) % countAdditionalIdles;
            yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    /// <summary>
    /// Включает состояние диалога в аниматоре и подписывается на событие окончания диалога.
    /// </summary>
    void ActivateDialoging()
    {
        _anim.SetBool("Dialoging", true);
        DialogManager.Instance.StoryEnded.AddListener(DeactivateDialoging);
    }

    /// <summary>
    /// Выключает состояние диалога в аниматоре и отписывается от события окончания диалога.
    /// </summary>
    public void DeactivateDialoging()
    {
        _anim.SetBool("Dialoging", false);
        DialogManager.Instance.StoryEnded.RemoveListener(DeactivateDialoging);
    }
}
