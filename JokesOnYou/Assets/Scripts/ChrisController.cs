using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChrisController : MonoBehaviour
{
    [SerializeField] Animator Anim;

    public UnityEvent OnHitEnded;

    public void GotPunched()
    {
        Anim.SetTrigger(AnimationParameters.GotHit);
    }

    public void MarkHitEnded()
    {
        OnHitEnded.Invoke();
    }
}
