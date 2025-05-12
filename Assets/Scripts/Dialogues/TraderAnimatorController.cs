using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PanelNPC))]
public class TraderAnimatorController : MonoBehaviour
{
    Animator _anim;

    [SerializeField][Range(1f, 4f)] int countAdditionalIdles;

    [SerializeField][Range(2f, 5f)] float delayNextIdle;

    int currentIdle;

    DialogableNPC npc;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        currentIdle = 0;
        StartCoroutine(ChangingIdles());

        npc = GetComponent<PanelNPC>();
        npc.DialogStarted.AddListener(ActivateDialoging);
    }

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

    void ActivateDialoging()
    {
        _anim.SetBool("Dialoging", true);
    }

    public void DeactivateDialoging()
    {
        _anim.SetBool("Dialoging", false);
    }
}
