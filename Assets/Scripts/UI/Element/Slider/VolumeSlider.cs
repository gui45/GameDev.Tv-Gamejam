using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : Slider
{
    [SerializeField]
    private AudioMixerGroup mixer;
    [SerializeField]
    private string SettingName;

    private new void Start()
    {

        slider.maxValue = 1;
        slider.minValue = 0.00001f;
        slider.value = PlayerPrefs.GetFloat(SettingName, 1);

        base.Start();
    }

    protected override void OnValueChange(float value)
    {
        PlayerPrefs.SetFloat(SettingName, value);
        mixer.audioMixer.SetFloat(SettingName, Mathf.Log10(value) * 20);
    }
}
