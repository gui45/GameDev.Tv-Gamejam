using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGhostElementManager : MonoBehaviour
{
    public GameObject realOnlySceneElements;
    public GameObject ghostOnlySceneElements;

    private bool ghost = false;
    private GameEvents gameEvents;

    private void Start()
    {
        gameEvents = GameEvents.instance;

        realOnlySceneElements.SetActive(true);
        ghostOnlySceneElements.SetActive(false);

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        gameEvents.OnGameOverEvent += OnGameOver;
        gameEvents.OnSwitchModeEvent += OnSwitchMode;
    }

    private void RemoveEvents()
    {
        gameEvents.OnGameOverEvent -= OnGameOver;
        gameEvents.OnSwitchModeEvent -= OnSwitchMode;
    }

    private void OnGameOver()
    {
        Destroy(this);
    }

    private void OnSwitchMode()
    {
        ghost = !ghost;

        if (ghost)
        {
            realOnlySceneElements.SetActive(false);
            ghostOnlySceneElements.SetActive(true);
        }
        else
        {
            realOnlySceneElements.SetActive(true);
            ghostOnlySceneElements.SetActive(false);
        }
    }

}
