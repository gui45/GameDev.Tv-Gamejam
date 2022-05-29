using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : UI
{
    [SerializeField]
    private ProgressBar playerHealth;
    [SerializeField]
    private ProgressBar ghostHealth;
    [SerializeField]
    private ProgressBar ghostTime;

    private GameSettings gameSettings;
    private float ghostCountDown;
    private bool ghost;

    private new void Start()
    {
        base.Start();

        gameSettings = SettingsRepository.instance.GameSettings;

        AddEvents();
    }

    private new void OnDestroy()
    {
        base.OnDestroy();

        RemoveEvents();
    }

    private void Update()
    {
        HandleGhostCountdown();
    }

    private void AddEvents()
    {
        gameEvents.OnSwitchModeEvent += OnSwithMode;
        gameEvents.OnPlayerHealthChangeEvent += OnPlayerHealthChange;
        gameEvents.OnPlayerGhostHealthChangeEvent += OnPlayerGhostHealthChange;
    }

    private void RemoveEvents()
    {
        gameEvents.OnSwitchModeEvent -= OnSwithMode;
        gameEvents.OnPlayerHealthChangeEvent -= OnPlayerHealthChange;
        gameEvents.OnPlayerGhostHealthChangeEvent -= OnPlayerGhostHealthChange;
    }

    private void HandleGhostCountdown()
    {
        ghostCountDown -= Time.deltaTime;

        if (ghostCountDown > 0)
        {
            ghostTime.SetFillLevel(ghostCountDown / gameSettings.MaxTimeAsGhost);
        }
        else
        {
            ghostTime.SetFillLevel(0);
        }
    }

    private void OnSwithMode()
    {
        ghost = !ghost;

        if (ghost)
        {
            ghostCountDown = gameSettings.MaxTimeAsGhost;
        }
        else
        {
            ghostCountDown = 0;
        }
    }

    private void OnPlayerHealthChange(float newAmount, float startAmount)
    {
        playerHealth.SetFillLevel(newAmount / startAmount);
    }

    private void OnPlayerGhostHealthChange(float newAmount, float startAmount)
    {
        ghostHealth.SetFillLevel(newAmount / startAmount);
    }
}
