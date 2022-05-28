using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeAsGhost;
    private bool ghost;
    private GameEvents gameEvents;

    private GameSettings settings;
    private void Start()
    {
        gameEvents = GameEvents.instance;
        settings = SettingsRepository.instance.GameSettings;

        Application.targetFrameRate = settings.FpsLimit;

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void Update()
    {
        if (ghost)
        {
            CountDownTimeAsGhost();
        }
    }

    private void AddEvents()
    {
        gameEvents.OnSwitchModeEvent += OnSwitchGhost;
    }

    private void RemoveEvents()
    {
        gameEvents.OnSwitchModeEvent -= OnSwitchGhost;
    }

    private void OnSwitchGhost()
    {
        ghost = !ghost;
        if (ghost)
        {
            timeAsGhost = 0;
        }
    }

    private void CountDownTimeAsGhost()
    {
        timeAsGhost += Time.deltaTime;

        if (timeAsGhost >= settings.MaxTimeAsGhost)
        {
            gameEvents.OnSwitchMode();
        }
    }

}
