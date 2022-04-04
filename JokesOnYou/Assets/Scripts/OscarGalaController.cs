using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OscarGalaController : MonoBehaviour
{
    #region Properties
    const float MaxProgress = 100f;
    [Range(0f, 100f)]
    public float CurrentProgress;

    //TODO:whats the score? time or points?
    public int Score = 0;


    //TODO: hook up animation, movement and cameras
    [SerializeField] Animator ChrisAnim;
    [SerializeField] Animator WillAnim;
    [SerializeField] Transform ChrisTransform;
    [SerializeField] Transform WillTransform;

    [SerializeField] Camera MainCamera;
    [SerializeField] Camera WillFaceCamera;
    [SerializeField] Camera ChrisFaceCamera;
    [SerializeField] Camera WillBackCamera;
    #endregion Properties

    public UnityEvent OnNextRound;
    public UnityEvent<int> OnGameEnded;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SlapProgress(float deltaValue)
    {
        CurrentProgress = Mathf.Clamp(CurrentProgress + deltaValue, 0, MaxProgress);

        var percent = CurrentProgress / MaxProgress;
        //Will gets angry and move forward
        if (deltaValue > 0)
        {
            Debug.Log($"Will get angry and move forward {percent}%");
            //TODO: play random angry gesture and move will forward (also manage cameras and face)
        }
        else if (deltaValue < 0)
        {
            Debug.Log($"Will calms down and backs up  {percent}%");
            //TODO: play random peacfull gesture and move will back to his seat (also manage cameras and face)
        }
        else
        {
            Debug.Log($"Will get reacts but stays in place {percent}%");
            //TODO: play random neutral gesture and move stays in position (also manage cameras and face)
        }

        //TODO:on animation played
        if (CurrentProgress >= MaxProgress)
        {
            Debug.Log($"Will reached the target. Current score {Score}.");
            OnGameEnded.Invoke(Score);
        }
        else
        {
            //Game not finished, run next round (show next dialog to choose)
            Debug.Log($"Chris speaks another dialog");
            OnNextRound.Invoke();
        }
    }
}
