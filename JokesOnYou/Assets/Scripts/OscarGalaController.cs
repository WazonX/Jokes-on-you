using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class OscarGalaController : MonoBehaviour
{
    #region Properties
    const float MaxProgress = 1f;
    [Range(0f, 1f)]
    public float CurrentProgress;

    //TODO:whats the score? time or points?
    public int Score = 0;

    enum GalaCameras
    {
        Wide,
        ChrisFace,
        WillFace
    }


    //TODO: hook up animation, movement and cameras
    [SerializeField] Animator ChrisAnim;
    [SerializeField] Animator WillAnim;
    [SerializeField] Transform ChrisTransform;
    [SerializeField] Transform WillTransform;

    [SerializeField] AnimationClip[] TalkingAnimations;
    List<int> AlreadyUsedTalkChrisAnimsIndex = new List<int>();

    [SerializeField] AnimationClip[] AngryAnimations;
    List<int> AlreadyUsedAngryWillAnimsIndex = new List<int>();

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera WideCam;
    [SerializeField] CinemachineVirtualCamera WillFaceCam;
    [SerializeField] CinemachineVirtualCamera ChrisFaceCam;

    [SerializeField] Transform StartingSpot;
    [SerializeField] Transform StoppingSpot;

    [SerializeField] ChrisController Chris;
    [SerializeField] WillController Will;

    //when snapping to postion while animation playing might mess up Will rotation and position we use this to reset
    Quaternion _willInitialRotation;

    public float TotalDistance;

    public bool IsWalking = false;
    public float SingleStepDuration = 0.515f;

    public float StopTimer;
    float _rootMotionTolerance = 0.025f;
    public float _spotTolerance = 0.025f;
    public float _progressTolerance = 0.025f;

    //Store last walk animation progress before pause, so we can resume to alternate steps left and right
    public float _lastWalkProgress;
    public bool _movingForward = true;
    DialogItem _currentDialogItem;


    #endregion Properties

    public UnityEvent OnNextRound;
    public UnityEvent<int> OnGameEnded;
    public UnityEvent<int> OnScoreChanged;

    void Start()
    {
        Assert.IsNotNull(ChrisAnim);
        Assert.IsNotNull(WillAnim);
        Assert.IsNotNull(Chris);
        Assert.IsNotNull(Will);

        SwitchCam(GalaCameras.Wide);

        TotalDistance = Vector3.Distance(StartingSpot.position, StoppingSpot.position);
        _willInitialRotation = WillTransform.localRotation;
        Chris.OnHitEnded.AddListener(OnHitEnded);
        Will.OnGestureFinished.AddListener(OnWillGestureFinished);
    }

    void Update()
    {
        distanceToStopSpot = (StoppingSpot.position - WillTransform.position).magnitude;
        if (IsWalking)
        {
            var clampedDistance = Mathf.Clamp(distanceToStopSpot, 0, TotalDistance);

            CurrentProgress = Mathf.Clamp((TotalDistance - clampedDistance) / TotalDistance, 0, MaxProgress);
            if (StopTimer > 0)
            {
                if (CurrentProgress >= MaxProgress - _progressTolerance)
                {
                    Debug.Log("Reached the tolerance endgame");
                    ToggleWalking(false);
                    CheckGameConditions();
                    return;
                }

                //Dont back up to public
                if (WillTransform.position.z > StartingSpot.position.z + _rootMotionTolerance)
                {
                    SetWill_Z(StartingSpot.position);
                    ToggleWalking(false);
                    CheckGameConditions();

                    Debug.Log("Dont back up more");
                    return;
                }
                //Dont walk past Chris
                if (WillTransform.position.z < StoppingSpot.position.z)
                {
                    SetWill_Z(StoppingSpot.position);
                    ToggleWalking(false);

                    Debug.Log($"Dont miss Chris WillZ{WillTransform.position.z} StopZ:{StoppingSpot.position.z}");
                    TriggetPunch();
                    return;
                }

                StopTimer -= Time.deltaTime;

                Debug.Log($"Update StopTimer:{StopTimer}");
            }
            else
            {
                ToggleWalking(false);
                CheckGameConditions();
                Debug.Log("Update StopTimer < 0");
            }
        }
    }
    public float distanceToStopSpot;

    public void SendDialogToGala(DialogItem dialogItem)
    {
        _currentDialogItem = dialogItem;

        ChrisSpeaks();
    }

    public void ChrisStoppedTalking()
    {
        ChrisAnim.SetBool(AnimationParameters.Param_IsTalking, false);
        ActivateWill();
    }

    public void ActivateWill()
    {
        SwitchCam(GalaCameras.WillFace);

        //gets angry
        if (_currentDialogItem.IsNegative)
        {
            string animationName;
            DrawAnimation(ref AlreadyUsedAngryWillAnimsIndex, AngryAnimations, out animationName);
            WillAnim.Play(animationName, 0);
        }
        else if(_currentDialogItem.IsPositive)
        {

        }
        else
        {

        }
    }

    void OnWillGestureFinished()
    {
        Debug.Log("OnWillGestureFinished");
        PerformWillMovement();
    }

    void PerformWillMovement()
    {
        Debug.Log("PerformWillMovement");
        var steps = _currentDialogItem.Value;
        var previousMovingForward = _movingForward;
        var currentMovingForward = true;

        //Will gets angry and move forward
        if (_currentDialogItem.IsNegative)
        {
            Debug.Log($"Will get angry and move forward");
            //TODO: play random angry gesture and move will forward (also manage cameras and face)
            Score += Mathf.FloorToInt(steps);
            OnScoreChanged.Invoke(Score);
        }
        else if (_currentDialogItem.IsPositive)
        {
            currentMovingForward = false;
            Debug.Log($"Will calms down and backs up");
            //TODO: play random peacfull gesture and move will back to his seat (also manage cameras and face)
        }
        else
        {
            Debug.Log($"Will get reacts but stays in place");
            //TODO: play random neutral gesture and move stays in position (also manage cameras and face)
        }

        if (currentMovingForward != previousMovingForward)
            _lastWalkProgress = 0;
        _movingForward = currentMovingForward;

        //TODO: gesture

        //if already on margin spots
        if (IsWillAroundPosition(StoppingSpot.position) && _movingForward)
        {
            Debug.Log("Already in position Stop");
            SetWill_Z(StoppingSpot.position);
            CheckGameConditions();
            return;
        }
        if (IsWillAroundPosition(StartingSpot.position) && !_movingForward)
        {
            Debug.Log("Already in position Start");
            SetWill_Z(StartingSpot.position);
            CheckGameConditions();
            return;
        }

        Walk(steps);
    }

    void CheckGameConditions()
    {
        //TODO:on animation played
        if (CurrentProgress >= MaxProgress - _progressTolerance || WillTransform.position.z < StoppingSpot.position.z)
        {
            TriggetPunch();
            Debug.Log($"Will reached the target. Current score {Score}.");
        }
        else
        {
            //Game not finished, run next round (show next dialog to choose)
            Debug.Log($"Chris speaks another dialog");
            SwitchCam(GalaCameras.Wide);
            OnNextRound.Invoke();
        }
    }

    void TriggetPunch()
    {
        WillAnim.SetTrigger(AnimationParameters.Trigger_Punch);
    }

    void OnHitEnded()
    {
        OnGameEnded.Invoke(Score);
    }

    void Walk(float steps)
    {
        SwitchCam(GalaCameras.WillFace);
        if (steps > 0)
        {
            StopTimer = steps * SingleStepDuration;
            ToggleWalking(true);
        }
        else if (steps < 0)
        {
            //go backwards
            StopTimer = steps * -1 * SingleStepDuration;
            ToggleWalking(true);
        }
        else
        {
            //do nothing
            CheckGameConditions();
        }
    }

    void ToggleWalking(bool toggle)
    {
        if (!toggle)
        {
            WillAnim.applyRootMotion = false;

            SwitchCam(GalaCameras.Wide);

            StopTimer = 0;

            //save walk animation progress to alternate steps when resumed
            AnimatorStateInfo animationState = WillAnim.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] myAnimatorClip = WillAnim.GetCurrentAnimatorClipInfo(0);
            _lastWalkProgress = myAnimatorClip[0].clip.length * animationState.normalizedTime;

            if (_movingForward)
                WillAnim.SetBool(AnimationParameters.Param_IsWalking, false);
            else
                WillAnim.SetBool(AnimationParameters.Param_IsBackingUp, false);

            ResetWill_Rotation();
        }
        else
        {
            WillAnim.applyRootMotion = true;
            //resume walk alternating steps
            if (_movingForward)
            {
                WillAnim.SetBool(AnimationParameters.Param_IsWalking, true);
                WillAnim.Play(AnimationParameters.Anim_Forward, 0, _lastWalkProgress);
            }
            else
            {
                WillAnim.SetBool(AnimationParameters.Param_IsBackingUp, true);
                WillAnim.Play(AnimationParameters.Anim_Backward, 0, _lastWalkProgress);
            }

        }
        IsWalking = toggle;
    }

    bool IsWillAroundPosition(Vector3 position)
    {
        return Vector3.Distance(WillTransform.position, position) <= _spotTolerance;
    }

    void SetWill_Z(Vector3 position)
    {
        WillTransform.position = new Vector3(WillTransform.position.x, WillTransform.position.y, position.z);
    }

    void ResetWill_Rotation()
    {
        WillTransform.localRotation = _willInitialRotation; ;
    }

    void SwitchCam(GalaCameras camera)
    {
        switch (camera)
        {
            case GalaCameras.Wide:
            default:
                WideCam.Priority = 1;
                ChrisFaceCam.Priority = 0;
                WillFaceCam.Priority = 0;
                break;
            case GalaCameras.ChrisFace:
                WideCam.Priority = 0;
                ChrisFaceCam.Priority = 1;
                WillFaceCam.Priority = 0;
                break;
            case GalaCameras.WillFace:
                WideCam.Priority = 0;
                ChrisFaceCam.Priority = 0;
                WillFaceCam.Priority = 1;
                break;
        }
    }

    void ChrisSpeaks()
    {
        SwitchCam(GalaCameras.ChrisFace);

        ChrisAnim.SetBool(AnimationParameters.Param_IsTalking, true);

        string animName;
        DrawAnimation(ref AlreadyUsedTalkChrisAnimsIndex, TalkingAnimations, out animName);

        Debug.Log($"Drawed Chris speaking anim:{animName}");
        ChrisAnim.Play(animName);
    }

    int DrawAnimation(ref List<int> alreadyDrawed, AnimationClip[] allAnimClipsPool, out string animName)
    {
        //Check if all positives already draw and we need to reshuffle dialogues
        if (alreadyDrawed.Count >= allAnimClipsPool.Length)
        {
            //reset memory
            alreadyDrawed.Clear();
        }

        int drawedIndex = -1;
        do
        {
            drawedIndex = Random.Range(0, allAnimClipsPool.Length);
        }
        while (alreadyDrawed.Contains(drawedIndex));
        alreadyDrawed.Add(drawedIndex);

        animName = allAnimClipsPool[drawedIndex].name;

        return drawedIndex;
    }
}
