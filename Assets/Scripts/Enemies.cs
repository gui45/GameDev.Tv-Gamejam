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
    private BoxCollider2D detectionAttack1;
    private Player playerInRange;


    private float health;
    private float currentDirection;
    private bool canAttack;
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
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            playerInRange = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && playerInRange == player)
        {
            playerInRange = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(DetectPlayerAttack1() && canAttack)
        {
            PrepAttack1();
        }

        if (!isAttacking)
        { 
            MovementUpdate();
            FlipUpdate();
        } 
    }

    private bool DetectPlayerAttack1()
    {
        if (playerInRange == null)
            return false;

        if (detectionAttack1.IsTouchingLayers(LayerMask.GetMask(gameSettings.PlayerLayer))
            && (spriteRenderer.flipX && playerInRange.transform.position.x < transform.position.x)
            || (!spriteRenderer.flipX && playerInRange.transform.position.x > transform.position.x))
        {
            return true;
        }

        return false;
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSecondsRealtime(settings.TimeBetweenAttack);
        canAttack = true;
    }

    private void Attack1()
    {
        canAttack = false;
        CauseDamage(settings.Attack1Damage);

        StartCoroutine(ResetAttack());
        isAttacking = false;
    }

    private void CauseDamage(float damage)
    {
        if (playerInRange == null)
            return;

        if (spriteRenderer.flipX && playerInRange.transform.position.x < transform.position.x)
        {
            if (playerInRange.TakeDamage(damage))
            {
                playerInRange = null;
            }
        }
        else if (!spriteRenderer.flipX && playerInRange.transform.position.x > transform.position.x)
        {
            if (playerInRange.TakeDamage(damage))
            {
                playerInRange = null;
            }
        }
    }

    private void PrepAttack1()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("IsPrepAttacking");
        }

    }

    private void PerformeAttack1()
    {
        if (DetectPlayerAttack1())
        {
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);
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

    public bool TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
