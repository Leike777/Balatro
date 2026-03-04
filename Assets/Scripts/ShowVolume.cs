using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowVolume : MonoBehaviour
{
    public TMP_Text text;
    private Slider slider;
    public void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(UpdateText);
    }

    void Start()
    {
        if (name == "Volume")
        {
            slider.value = Settings.Instance.jsonSettings.volume / 100f;
        }
        else
        {
            slider.value = Settings.Instance.jsonSettings.soundEffectVolume / 100f;
        }
        
    }

    private void UpdateText(float value)
    {
        value *= 100f;
        text.text = Mathf.FloorToInt(value).ToString();
        
        if (name == "Volume")
        {
            Settings.Instance.SetVolume(Mathf.FloorToInt(value));
        }
        else
        {
            Settings.Instance.SetSoundEffectVolume(Mathf.FloorToInt(value));
        }
    }
}
