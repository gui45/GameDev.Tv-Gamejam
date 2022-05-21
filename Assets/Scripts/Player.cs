using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerSettings settings;
    private GameSettings gameSettings;
    [SerializeField]
    private BoxCollider2D feets;
    private Animator animator;
    private Rigidbody2D rb;
    private GameEvents gameEvents;
    private SpriteRenderer spriteRenderer;

    private float health;
    private float currentSpeed;
    private bool isOnGround;

    private void Start()
    {
        gameSettings = SettingsRepository.instance.GameSettings;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameEvents = GameEvents.instance;
        AddEvents();

        health = settings.Health;
    }

    private void AddEvents()
    {
        gameEvents.onJumpEvent += OnJump;
        gameEvents.onMoveEvent += OnMove;
        gameEvents.onLookEvent += OnLook;
    }

    private void RemoveEvents()
    {
        gameEvents.onJumpEvent -= OnJump;
        gameEvents.onMoveEvent -= OnMove;
        gameEvents.onLookEvent -= OnLook;
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void Update()
    {
        MovementUpdate();
        CheckIfOnGround();
    }

    private void CheckIfOnGround()
    {
        isOnGround = feets.IsTouchingLayers(LayerMask.GetMask(gameSettings.GroundLayer));
    }

    private void MovementUpdate()
    {
        if (currentSpeed != 0 && isOnGround)
        {
            animator.SetInteger("AnimState", 1);
            rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.deltaTime, 0);
        }
    }

    private void OnMove(float speed)
    {
        if (speed != 0)
        {
            spriteRenderer.flipX = speed < 0;
        }

        currentSpeed = speed;
    }

    private void OnJump()
    {

    }

    private void OnLook(Vector2 vector)
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
