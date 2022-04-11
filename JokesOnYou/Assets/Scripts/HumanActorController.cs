using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HumanActorController : MonoBehaviour
{
    public enum FacialExpression
    {
        Neutral,
        Angry,
        Happy,
        Talking
    }

    [SerializeField] protected Animator _animator;
    [SerializeField] SkinnedMeshRenderer _mesh;
    [SerializeField] Texture _talkingTex;
    [SerializeField] Texture _neutralTex;
    [SerializeField] Texture _angryTex;
    [SerializeField] Texture _happyTex;

    public Texture TalkingTex => _talkingTex;
    public Texture NeutralTex => _neutralTex;
    public Texture AngryTex => _angryTex;
    public Texture HappyTex => _happyTex;

    public UnityEvent OnHitLanded;
    public UnityEvent OnHitEnded;
    public UnityEvent OnGestureFinished;

    public virtual void HitReached()
    {
    }
    public void GotPunched()
    {
        _animator.SetTrigger(AnimationParameters.Trigger_GotHit);
        OnHitLanded.Invoke();
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

    public void SwapFacialTexture(FacialExpression facialExpression)
    {
        var mats = _mesh.materials;
        var headMat = mats[4];

        var newText = _neutralTex;
        switch (facialExpression)
        {
            case FacialExpression.Neutral:
            default:
                newText = _neutralTex;
                break;
            case FacialExpression.Angry:
                newText = _angryTex;
                break;
            case FacialExpression.Happy:
                newText = _happyTex;
                break;
            case FacialExpression.Talking:
                newText = _talkingTex;
                break;
        }

        headMat.mainTexture = newText;
        _mesh.materials = mats;
    }
}
