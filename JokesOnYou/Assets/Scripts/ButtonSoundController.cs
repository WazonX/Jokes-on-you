using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        
        if (button != null && GameMusic.instance != null)
        {
            button.onClick.AddListener(() => { GameMusic.instance.PlaySFX(GameMusic.instance.DefaultClick); });
        }
    }
}
