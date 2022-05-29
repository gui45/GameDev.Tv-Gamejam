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
    public event Action OnRollEvent;
    public event Action OnBlockEvent;
    public event Action OnSwitchModeEvent;
    public event Action OnInteractEvent;

    public event Action OnGameOverEvent;
    public event Action<float, float> OnPlayerHealthChangeEvent;
    public event Action<float, float> OnPlayerGhostHealthChangeEvent;
    public event Action<UI> OnNextUIEvent;
    public event Action<int> OnNextSceneEvent;
    public event Action OnUnlockGhostEvent;

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

    public void OnGhostUnlock()
    {
        OnUnlockGhostEvent?.Invoke();
    }
    public void OnNextScene(int scene)
    {
        OnNextSceneEvent?.Invoke(scene);
    }

    public void OnNextUI(UI ui)
    {
        OnNextUIEvent?.Invoke(ui);
    }

    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke();
    }

    public void OnPlayerHealthChange(float newAmount, float startAmount)
    {
        OnPlayerHealthChangeEvent?.Invoke(newAmount, startAmount);
    }

    public void OnPlayerGhostHealthChange(float newAmount, float startAmount)
    {
        OnPlayerGhostHealthChangeEvent?.Invoke(newAmount, startAmount);
    }

    // INPUTS

    public void OnInteract()
    {
        OnInteractEvent?.Invoke();
    }

    public void OnSwitchMode()
    {
        if (GetComponent<GameManager>().ghostUnlocked)
        {
            OnSwitchModeEvent?.Invoke();
        }
    }

    public void OnBlock()
    {
        OnBlockEvent?.Invoke();
    }

    public void OnRoll()
    {
        OnRollEvent?.Invoke();
    }

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
        onJumpEvent?.Invoke();
    }

    public void OnMove(InputValue input)
    {
        onMoveEvent?.Invoke(input.Get<float>());
    }
}
