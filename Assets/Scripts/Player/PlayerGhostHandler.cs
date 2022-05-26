using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostHandler : MonoBehaviour
{
    public Player player;
    public PlayerGhost playerGhost;

    private bool ghost = false;
    private GameEvents gameEvents;

    void Start()
    {
        gameEvents = GameEvents.instance;

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
            playerGhost.transform.position = player.transform.position;
        }
    }
}
