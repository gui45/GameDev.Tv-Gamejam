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
    private Collider2D feets;
    private Animator animator;
    private Rigidbody2D rb;
    private GameEvents gameEvents;
    private SpriteRenderer spriteRenderer;

    private float health;
    private float currentSpeed;
    private bool isOnGround;
    private float offGrounfDelay;
    private bool isFalling;
    private float attackCoolDown;
    private bool primaryAttackFlip = false;
    private List<Enemies> enemiesInRange = new List<Enemies>();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemies enemy = collision.GetComponent<Enemies>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemies enemy = collision.GetComponent<Enemies>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void AddEvents()
    {
        gameEvents.onJumpEvent += OnJump;
        gameEvents.onMoveEvent += OnMove;
        gameEvents.OnPrimaryActionEvent += OnPrimaryAttack;
        gameEvents.OnSecondaryActionEvent += OnSecondaryActtack;
    }

    private void RemoveEvents()
    {
        gameEvents.onJumpEvent -= OnJump;
        gameEvents.onMoveEvent -= OnMove;
        gameEvents.OnPrimaryActionEvent -= OnPrimaryAttack;
        gameEvents.OnSecondaryActionEvent -= OnSecondaryActtack;
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void Update()
    {
        CheckOnGround();
        CheckIsFalling();
        HandleAttackCoolDown();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void HandleAttackCoolDown()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
    }

    private void OnPrimaryAttack()
    {
        if (attackCoolDown <= 0)
        {
            attackCoolDown += settings.PrimaryAttackCoolDown;
            if (primaryAttackFlip)
            {
                animator.SetTrigger("Attack1");
            }
            else
            {
                animator.SetTrigger("Attack2");
            }
            primaryAttackFlip = !primaryAttackFlip;
            CauseDamage(settings.PrimaryAttackDamage);
        }
    }

    private void OnSecondaryActtack()
    {
        if (attackCoolDown <= 0)
        {
            animator.SetTrigger("Attack3");
            attackCoolDown += settings.SecondaryAttackCoolDown;
            CauseDamage(settings.SecondaryAttackDamage);
        }
    }

    private void CauseDamage(float damage)
    {
        List<Enemies> toRemove = new List<Enemies>();
        foreach (Enemies enemy in enemiesInRange)
        {
            if (spriteRenderer.flipX && enemy.transform.position.x < transform.position.x)
            {
                if (enemy.TakeDamage(damage))
                {
                    toRemove.Add(enemy);
                }
            }
            else if (!spriteRenderer.flipX && enemy.transform.position.x > transform.position.x)
            {
                if (enemy.TakeDamage(damage))
                {
                    toRemove.Add(enemy);
                }
            }
        }

        foreach (Enemies enemy in toRemove)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void CheckIsFalling()
    {
        if (isOnGround)
        {
            offGrounfDelay = 0;
        }
        else
        {
            offGrounfDelay += Time.deltaTime;
        }

        if (offGrounfDelay >= settings.OffGroundDelayToFall)
        {
            isFalling = true;
            animator.SetBool("Grounded", false);
            animator.SetFloat("AirSpeedY", rb.velocity.y);
        }
        else
        {
            isFalling = false;
            animator.SetBool("Grounded", true);
            animator.SetFloat("AirSpeedY", 0);
        }
    }

    private void CheckOnGround()
    {
        isOnGround = feets.IsTouchingLayers(LayerMask.GetMask(gameSettings.GroundLayer));
    }

    private void MovementUpdate()
    {
        //MOVE
        if (currentSpeed != 0 && !isFalling)
        {
            animator.SetInteger("AnimState", 1);
            rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if(settings.CanMoveWhenFalling && currentSpeed != 0)
        {
            rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime * settings.XSpeedModiferFalling, rb.velocity.y);
            animator.SetInteger("AnimState", 0);
        }
        //STOP MOVING
        else if(settings.PlayerStopsWhenKeyUp && currentSpeed == 0)
        {
            animator.SetInteger("AnimState", 0);

            if (isFalling)
            {
                if (settings.CanMoveWhenFalling)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            animator.SetInteger("AnimState", 0);
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
        if (!isFalling)
        {
            rb.AddForce(new Vector2(0, settings.JumpForce));
            animator.SetTrigger("Jump");
        }
    }

    private void Die()
    {
        Destroy(gameObject);
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
}
