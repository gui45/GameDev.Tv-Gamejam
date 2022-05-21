using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;
using System;

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
    private BoxCollider2D rightSideAttack1;
    [SerializeField]
    private BoxCollider2D leftSideAttack1;


    private float health;
    private float currentDirection;
    private bool isAttacking;

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
        isAttacking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DetectPlayer())
        {
            StartCoroutine(WaitBeforeAttack());
        }

        if (!isAttacking)
        { 
            MovementUpdate();
            FlipUpdate();
        } 
    }

    private bool DetectPlayer()
    {
        BoxCollider2D lookingSide;
        if (IsFacingRight())
        {
            lookingSide = rightSideAttack1;
        }
        else
        {
            lookingSide = leftSideAttack1;
        }

        if (lookingSide.IsTouchingLayers(LayerMask.GetMask(gameSettings.PlayerLayer)))
        {
            return true;
        }

        return false;
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private IEnumerator WaitBeforeAttack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(settings.WaitTimeBeforeAttack);

        if (DetectPlayer())
        {
            Attack();
        }
        else
        {
            isAttacking = false;
        }
    }

    private void FlipUpdate()
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
