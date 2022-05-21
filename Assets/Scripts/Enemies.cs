using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemies : MonoBehaviour
{
    [SerializeField]
    private EnemiesSettings settings;
    private Rigidbody2D rb;
    private Animator animator;
    private GameEvents gameEvents;
    private GameSettings gameSettings;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D rightSideLookingWall;
    [SerializeField]
    private BoxCollider2D leftSideLookingWall;

    [SerializeField]
    private BoxCollider2D rightSideLookingPlayer;
    [SerializeField]
    private BoxCollider2D leftSideLookingPlayer;


    private float health;
    private float currentDirection;
    private bool isOnGround;

    [SerializeField]
    LayerMask[] layerMasksFlip;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = SettingsRepository.instance.GameSettings;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        health = settings.Health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectPlayer();

        MovementUpdate();
        FlipUpdate();
    }

    private void DetectPlayer()
    {
        BoxCollider2D lookingSide;
        if (IsFacingRight())
        {
            lookingSide = rightSideLookingWall;
        }
        else
        {
            lookingSide = leftSideLookingWall;
        }

    }

    private void FlipUpdate()
    {
        BoxCollider2D lookingSide;
        if (IsFacingRight())
        {
            lookingSide = rightSideLookingPlayer;
        }
        else
        {
            lookingSide = leftSideLookingPlayer;
        }

        if (lookingSide.IsTouchingLayers(LayerMask.GetMask(gameSettings.PlayerLayer)))
        {
            Flip();
        }
    }

    private void MovementUpdate()
    {
        if (IsFacingRight())
        {
            currentDirection = 1;
        }
        else
        {
            currentDirection = -1;
        }

        //animator.SetInteger("AnimState", 1);
        rb.velocity = new Vector2(currentDirection * settings.MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private bool IsFacingRight()
    {
        return !spriteRenderer.flipX;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
