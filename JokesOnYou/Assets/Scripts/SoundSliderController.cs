using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] string _mixerParameter;

    void Start()
    {
        var volumeVal = PlayerPrefs.GetFloat(_mixerParameter, 1);
        _slider.value = Mathf.Pow(10f, volumeVal/20);

        if (_slider != null)
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
        }
    }

    void OnValueChanged(float value)
    {
        if (GameMusic.instance != null)
        {
            var sliderToMixer = Mathf.Log10(value) * 20;
            GameMusic.instance.SetSoundParameter(_mixerParameter, sliderToMixer);
        }
    }
}