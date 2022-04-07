using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Dialog", menuName = "JokesOnYou/DialogItem", order = 1)]
[CreateAssetMenu(fileName = "DialogConfig", menuName = "JokesOnYou/DialogConfig", order = 1)]
public class DialogConfig : ScriptableObject
{
    public DialogItem[] Positive;
    public DialogItem[] Negative;
    public DialogItem[] Neutral;
}

[Serializable()]
public struct DialogItem
{
    public string Text;
    public float Value;
    public float Duration;

    public bool IsNegative => Value > 0;
    public bool IsPositive => Value < 0;
    public bool IsNeutral => Value == 0;
}