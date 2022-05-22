using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { get; private set; }

    public event Action<float> onMoveEvent;
    public event Action onJumpEvent;
    public event Action OnPrimaryActionEvent;
    public event Action OnSecondaryActionEvent;

    public event Action OnGameOverEvent;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Game Event instance is already set");
        }
        else
        {
            instance = this;
        }
    }


    // GAME EVENTS

    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke();
    }

    // INPUTS

    public void OnPrimaryAction()
    {
        OnPrimaryActionEvent?.Invoke();
    }

    public void OnSecondaryAction()
    {
        OnSecondaryActionEvent?.Invoke();
    }

    public void OnJump()
    {
        //Debug.Log("jump");
        onJumpEvent?.Invoke();
    }

    public void OnMove(InputValue input)
    {
        //Debug.Log("Move " + input.Get<float>());
        onMoveEvent?.Invoke(input.Get<float>());
    }
}
