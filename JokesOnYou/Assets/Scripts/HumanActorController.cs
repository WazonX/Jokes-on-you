using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HumanActorController : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public UnityEvent OnHitEnded;
    public UnityEvent OnGestureFinished;

    public virtual void HitReached()
    {
    }
    public void GotPunched()
    {
        _animator.SetTrigger(AnimationParameters.Trigger_GotHit);
    }

    public void MarkHitEnded()
    {
        OnHitEnded.Invoke();
    }

    public void GestureFinished()
    {
        Debug.Log("Gesture Finished");
        OnGestureFinished.Invoke();
    }
}
