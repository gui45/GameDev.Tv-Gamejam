using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private CameraSettings settings;
    private PlayerGhostHandler playerGhostHandler;
    private GameEvents gameEvents;

    private bool ghost = false;
    private CameraStates state = CameraStates.FOLLOWING;
    private float returnDelay = 0;
    private float returnDelayStart;
    private Vector2 startingReturnPosition;

    private void Start()
    {
        returnDelayStart = settings.ReturnDelay;
        gameEvents = GameEvents.instance;
        playerGhostHandler = FindObjectOfType<PlayerGhostHandler>();

        AddEvents();
    }

    private void Update()
    {
        EvalState();

        if (playerGhostHandler != null && state == CameraStates.FOLLOWING)
        {
            FollowPlayer();
        }

        if (state == CameraStates.RETURN_TRANSITION)
        {
            ReturnPlayer();
        }
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

    private void EvalState()
    {
        if (returnDelay > 0)
        {
            state = CameraStates.RETURN_TRANSITION;
        }
        else
        {
            state = CameraStates.FOLLOWING;
        }
    }

    private void ReturnPlayer()
    {
        returnDelay -= Time.deltaTime;

        Vector2 nextPosition = Vector2.Lerp(playerGhostHandler.player.transform.position, startingReturnPosition,  returnDelay / returnDelayStart);

        transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);
    }

    private void FollowPlayer()
    {
        if (ghost)
        {
            transform.position = new Vector3(playerGhostHandler.playerGhost.transform.position.x, playerGhostHandler.playerGhost.transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(playerGhostHandler.player.transform.position.x, playerGhostHandler.player.transform.position.y, transform.position.z);
        }
    }
    private void OnSwitchMode()
    {
        ghost = !ghost;

        if (!ghost)
        {
            returnDelay = settings.ReturnDelay;
            startingReturnPosition = transform.position;
        }
    }

    private void OnGameOver()
    {
        Destroy(this);
    }
}
