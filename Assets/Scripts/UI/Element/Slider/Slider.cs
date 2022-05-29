using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public abstract class Slider : MonoBehaviour
{
    protected UnityEngine.UI.Slider slider;

    protected void Awake()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
    }

    protected void Start()
    {
        slider.onValueChanged.AddListener(OnValueChange);
    }

    protected abstract void OnValueChange(float value);
}
