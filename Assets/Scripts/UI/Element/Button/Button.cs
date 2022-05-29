using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public abstract class Button : MonoBehaviour
{
    private UnityEngine.UI.Button button;

    protected GameEvents gameEvents;

    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        gameEvents = GameEvents.instance;

        button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
