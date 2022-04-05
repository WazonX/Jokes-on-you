using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] DialogConfig DialogConfig;
    [SerializeField] OscarGalaController GalaController;
    [SerializeField] AudioClip[] ChrisSounds;
    [SerializeField] AudioClip[] WillSounds;
    [SerializeField] AudioSource AudioSource;


    List<int> AlreadyDrawedPositiveDialogues = new List<int>();
    List<int> AlreadyDrawedNegativeDialogues = new List<int>();
    List<int> AlreadyDrawedNeutralDialogues = new List<int>();
    List<int> ButtonIndexAlreadyDrawedThisRound = new List<int>();
    const int ButtonCount = 3;

    [SerializeField] TextMeshProUGUI CurrentScoreTextBox;
    [SerializeField] FadeController MenuVisuals;

    [Header("Dialog selection")]
    [SerializeField] ChrisDialogButton[] DialogButtons;
    [SerializeField] FadeController SelectorVisuals;

    [Header("Speaker")]
    [SerializeField] FadeController SpeakerVisuals;
    [SerializeField] TextMeshProUGUI SpeechTextBox;
    [SerializeField] Button SkipButton;
    float _countdown;
    bool _isSpeaking;


    [Header("EndGameScreen")]
    [SerializeField] string ScoreText = "Was Smith took {0} steps to get to you";
    [SerializeField] TextMeshProUGUI ScoreTextBox;
    [SerializeField] FadeController EndScreen;

    DialogItem _currentDialogItem;


    void Start()
    {
        Assert.IsNotNull(DialogConfig, "Dialog Data not set");
        Assert.IsNotNull(DialogButtons, "DialogButtons not set");

        Assert.IsNotNull(SelectorVisuals);

        Assert.IsNotNull(SpeakerVisuals);
        Assert.IsNotNull(SpeechTextBox);
        Assert.IsNotNull(SkipButton);


        //Setup for advanced button manipulation
        foreach (var chDButton in DialogButtons)
        {
            chDButton.Setup(this);
            chDButton.AdvancedClick.AddListener(ButtonClicked);
        }

        //Hook to Gala Controller Events
        GalaController.OnNextRound.AddListener(GalaRequestedNextRound);
        GalaController.OnGameEnded.AddListener(GalaEnded);
        GalaController.OnScoreChanged.AddListener(GalaScoreChanged);

        SkipButton.onClick.AddListener(SkipSpeaker);

        ShowDialogues();
        NextQuestion();
    }

    private void Update()
    {
        if (_isSpeaking)
        {
            if (_countdown > 0)
            {
                _countdown -= Time.deltaTime;
            }
            else
            {
                _isSpeaking = false;
                HideDialogues();
                GalaController.ActivateWill();
            }
        }
    }


    public void NextQuestion()
    {
        ButtonIndexAlreadyDrawedThisRound.Clear();

        //DrawPositive
        DialogItem drawedPositiveData;
        Draw(ref AlreadyDrawedPositiveDialogues, DialogConfig.Positive, out drawedPositiveData);
        var positiveButton = DrawButton(ref ButtonIndexAlreadyDrawedThisRound, DialogButtons);
        positiveButton.SetDialog(drawedPositiveData);

        //DrawNegative
        DialogItem drawedNegativeData;
        Draw(ref AlreadyDrawedNegativeDialogues, DialogConfig.Negative, out drawedNegativeData);
        var negativeButton = DrawButton(ref ButtonIndexAlreadyDrawedThisRound, DialogButtons);
        negativeButton.SetDialog(drawedNegativeData);

        //DrawNegative
        DialogItem drawedNeutralData;
        Draw(ref AlreadyDrawedNeutralDialogues, DialogConfig.Neutral, out drawedNeutralData);
        var neutralButton = DrawButton(ref ButtonIndexAlreadyDrawedThisRound, DialogButtons);
        neutralButton.SetDialog(drawedNeutralData);

        ButtonIndexAlreadyDrawedThisRound.Clear();
    }

    int Draw(ref List<int> alreadyDrawed, DialogItem[] allDialoguePool, out DialogItem item)
    {
        //Check if all positives already draw and we need to reshuffle dialogues
        if (alreadyDrawed.Count >= allDialoguePool.Length)
        {
            //reset memory
            alreadyDrawed.Clear();
        }

        int drawedIndex = -1;
        do
        {
            drawedIndex = Random.Range(0, allDialoguePool.Length);
        }
        while (alreadyDrawed.Contains(drawedIndex));
        alreadyDrawed.Add(drawedIndex);

        item = allDialoguePool[drawedIndex];

        return drawedIndex;
    }

    ChrisDialogButton DrawButton(ref List<int> alreadyDrawed, ChrisDialogButton[] buttons)
    {
        //Check if can draw any more buttons
        if (alreadyDrawed.Count >= buttons.Length)
        {
            //reset memory
            alreadyDrawed.Clear();
        }

        int drawedIndex = -1;
        do
        {
            drawedIndex = Random.Range(0, buttons.Length);
        }
        while (alreadyDrawed.Contains(drawedIndex));
        alreadyDrawed.Add(drawedIndex);

        return buttons[drawedIndex];
    }

    void GalaRequestedNextRound()
    {
        Debug.Log("GalaRequestedNextRound");
        ShowDialogues();
        ShowSelector();
        HideSpeaker();
        NextQuestion();
    }
    void GalaEnded(int score)
    {
        HideDialogues();

        //TODO:Show score and button to restart game
        ShowScore(score);
    }
    void GalaScoreChanged(int score)
    {
        CurrentScoreTextBox.text = score.ToString();
    }

    void ShowDialogues()
    {
        MenuVisuals.DesiredAlpha = 1;   
    }

    void HideDialogues()
    {
        Debug.Log($"HideDialogues CurA:{MenuVisuals.CurrentAlpha}");
        MenuVisuals.DesiredAlpha = 0;
    }

    void ShowSelector()
    {
        SelectorVisuals.DesiredAlpha = 1;
    }
    void HideSelector()
    {
        SelectorVisuals.DesiredAlpha = 0;
    }

    void ShowSpeaker()
    {
        _countdown = _currentDialogItem.Duration;
        _isSpeaking = true;
        SpeechTextBox.text = _currentDialogItem.Text;
        SpeakerVisuals.DesiredAlpha = 1;
    }

    void HideSpeaker()
    {
        SpeakerVisuals.DesiredAlpha = 0;
    }

    void SkipSpeaker()
    {
        _countdown = 0;
    }

    void ButtonClicked(DialogItem item)
    {
        _currentDialogItem = item;
        HideSelector();
        ShowSpeaker();
        GalaController.SendDialogToGala(_currentDialogItem);
    }

    public void ShowScore(int score)
    {
        HideDialogues();
        EndScreen.gameObject.SetActive(true);
        ScoreTextBox.text = string.Format(ScoreText, score);
        EndScreen.DesiredAlpha = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
