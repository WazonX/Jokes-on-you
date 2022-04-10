using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Serializable()]
    public class TutorialStep
    {
        public GameObject StepContent;
        public Toggle StepToggle;
    }

    int _currentIndex = 0;


    [SerializeField]
    TutorialStep[] _tutorialSteps;

    private void Start()
    {
        for (int i = 0; i < _tutorialSteps.Length; ++i)
        {
            _tutorialSteps[i].StepToggle.onValueChanged.AddListener(TogglesChanged);
        }
    }

    void TogglesChanged(bool value)
    {
        Debug.Log($"Toggle change:{value}");
        for (int i = 0; i < _tutorialSteps.Length; ++i)
        {
            if (_tutorialSteps[i].StepToggle.isOn)
            {
                _currentIndex = i;
                DisplayCurrentStep();
                Debug.Log($"Found current:{i}");
                return;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextStep();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PrevStep();
        }
    }

    void NextStep()
    {
        if (_currentIndex < _tutorialSteps.Length - 1)
        {
            ++_currentIndex;
        }
        else
        {
            _currentIndex = _tutorialSteps.Length - 1;
        }
        _tutorialSteps[_currentIndex].StepToggle.OnSubmit(new BaseEventData(EventSystem.current));
        DisplayCurrentStep();
    }

    void PrevStep()
    {
        if (_currentIndex > 0)
        {
            --_currentIndex;
        }
        else
        {
            _currentIndex = 0;
        }
        _tutorialSteps[_currentIndex].StepToggle.OnSubmit(new BaseEventData(EventSystem.current));
        DisplayCurrentStep();
    }

    void DisplayCurrentStep()
    {
        for (int i = 0; i < _tutorialSteps.Length; ++i)
        {
            _tutorialSteps[i].StepContent.gameObject.SetActive(i == _currentIndex);
            if(_currentIndex == i)
            {
                //_tutorialSteps[i].StepToggle.isOn = true;
                _tutorialSteps[i].StepToggle.OnSelect(new UnityEngine.EventSystems.BaseEventData(EventSystem.current));
            }
        }
    }
}
