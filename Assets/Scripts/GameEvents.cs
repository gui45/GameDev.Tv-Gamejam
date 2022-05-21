using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { get; private set; }

    public event Action<Vector2> onLookEvent;
    public event Action<float> onMoveEvent;
    public event Action onJumpEvent;

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

    public void OnLook(InputValue input)
    {
        //Debug.Log("Look " + input.Get<Vector2>());
        onLookEvent?.Invoke(input.Get<Vector2>());
    }
}
