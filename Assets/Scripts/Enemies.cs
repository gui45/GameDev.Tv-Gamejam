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
    private BoxCollider2D rightSide;
    [SerializeField]
    private BoxCollider2D leftSide;
    private BoxCollider2D lookingSide;


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
    void Update()
    {

        MovementUpdate();

        FlipUpdate();
    }

    private void FlipUpdate()
    {
        if (IsFacingRight())
        {
            lookingSide = rightSide;
        }
        else
        {
            lookingSide = leftSide;
        }

        foreach (var layerMask in layerMasksFlip)
        {
            if (lookingSide.IsTouchingLayers(layerMask))
            {
                Flip();
            }
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
        rb.velocity = new Vector2(currentDirection * settings.MovementSpeed * Time.deltaTime, rb.velocity.y);
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
