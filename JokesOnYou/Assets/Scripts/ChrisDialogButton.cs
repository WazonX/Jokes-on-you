using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChrisDialogButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DialogTextBox;
    [SerializeField] Button DialogButton;
    public DialogItem Data;

    public UnityEvent<DialogItem> AdvancedClick;
    DialogController _controller;

    void Start()
    {
        Assert.IsNotNull(DialogTextBox, "DialogText not set");
        Assert.IsNotNull(DialogButton, "DialogButton not set");

        //Native button call our event
        DialogButton.onClick.AddListener(() => { AdvancedClick.Invoke(Data); });
    }

    void Update()
    {
        
    }

    public void Setup(DialogController controller)
    {
        _controller = controller;
    }

    public void SetDialog(DialogItem item)
    {
        Data = item;
        DialogTextBox.text = Data.Text;
    }
}
