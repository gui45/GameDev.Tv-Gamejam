using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerSettings settings;
    private Animator animator;
    private Rigidbody2D rb;
    private GameEvents gameEvents;

    private float health;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameEvents = GameEvents.instance;
        AddEvents();

        health = settings.Health;
    }

    private void AddEvents()
    {
        
    }

    private void RemoveEvents()
    {

    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void OnMove()
    {

    }

    private void OnJump()
    {

    }

    private void OnLook()
    {

    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Die();
        }
    }
}
