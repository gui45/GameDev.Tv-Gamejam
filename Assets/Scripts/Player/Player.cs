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
    private CapsuleCollider2D feets;
    private Animator animator;
    private Rigidbody2D rb;
    private GameEvents gameEvents;
    private SpriteRenderer spriteRenderer;

    private float health;
    private float currentSpeed;
    private float offGrounfDelay;
    private float attackCoolDown;
    private bool primaryAttackFlip = false;
    private List<Enemies> enemiesInRange = new List<Enemies>();
    private PlayerStates state = PlayerStates.IDLE;
    private float deadForSeconds = 0;
    private float staggerDelay = 0;
    private float rollDuration = 0;

    private void Start()
    {
        feets = GetComponent<CapsuleCollider2D>();
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
        gameEvents.OnRollEvent += OnRoll;
        gameEvents.onJumpEvent += OnJump;
        gameEvents.onMoveEvent += OnMove;
        gameEvents.OnPrimaryActionEvent += OnPrimaryAttack;
        gameEvents.OnSecondaryActionEvent += OnSecondaryActtack;
    }

    private void RemoveEvents()
    {
        gameEvents.OnRollEvent -= OnRoll;
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
        if (state != PlayerStates.DYING)
        {
            HandeHurtStagger();
            HandleAttackCoolDown();
            CheckOnGround();

            EvalState();
        }
        else
        {
            DyingUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (state != PlayerStates.DYING)
        {
            MovementUpdate();
            HandleRoll();
        }
    }

    private void EvalState()
    {
        if (health <= 0 )
        {
            state = PlayerStates.DYING;
        }
        else if (staggerDelay > 0)
        {
            rollDuration = 0;
            currentSpeed = 0;
            state = PlayerStates.STAGGERED;
        }
        else if (attackCoolDown > 0)
        {
            state = PlayerStates.ATTACKING;
        }
        else if (rollDuration > 0)
        {
            state = PlayerStates.ROLLING;
        }
        else if (offGrounfDelay > 0 )
        {
            if (offGrounfDelay > settings.OffGroundDelayToFall)
            {
                state = PlayerStates.FALLING;
            }
            else
            {
                state = PlayerStates.OFFGROUND;
            }
        }
        else if (currentSpeed != 0)
        {
            state = PlayerStates.MOVING;
        }
        else
        {
            state = PlayerStates.IDLE;
        }
    }

    private void DyingUpdate()
    {
        AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        if (clip.length <= deadForSeconds)
        {
            gameEvents.OnGameOver();
            Destroy(this);
        }
    }

    private void HandeHurtStagger()
    {
        if (staggerDelay > 0)
        {
            staggerDelay -= Time.deltaTime;
        }
        else
        {
            staggerDelay = 0;
        }
    }

    private void HandleAttackCoolDown()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
    }

    private void HandleRoll()
    {
        if (rollDuration > 0)
        {
            rollDuration -= Time.deltaTime;
            float xForce = spriteRenderer.flipX ? -settings.RollForce : settings.RollForce;
            rb.velocity = new Vector2(xForce * Time.fixedDeltaTime, rb.velocity.y);

            if (rollDuration <= 0)
            {
                rollDuration = 0;

                feets.offset = new Vector2(feets.offset.x, feets.offset.y * 2);
                feets.size = new Vector2(feets.size.x, feets.size.y * 2);

                gameObject.layer = LayerMask.NameToLayer(settings.PlayerLayer);
            }
        }
    }

    private void OnRoll()
    {
        if (state != PlayerStates.STAGGERED && state != PlayerStates.FALLING && state != PlayerStates.DYING && state != PlayerStates.ATTACKING)
        {
            animator.SetTrigger("Roll");
            feets.offset = new Vector2(feets.offset.x, feets.offset.y / 2);
            feets.size = new Vector2(feets.size.x, feets.size.y / 2);

            rollDuration = settings.RollDuration;

            gameObject.layer = LayerMask.NameToLayer(settings.PlayerInvulnLayer);
        }
    }

    private void OnPrimaryAttack()
    {
        if (attackCoolDown <= 0 && state != PlayerStates.STAGGERED && state != PlayerStates.DYING && state != PlayerStates.ROLLING)
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
        if (attackCoolDown <= 0 && state != PlayerStates.STAGGERED && state != PlayerStates.DYING && state != PlayerStates.ROLLING)
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

    private void CheckOnGround()
    {
        if (feets.IsTouchingLayers(LayerMask.GetMask(gameSettings.GroundLayer)))
        {
            if (offGrounfDelay > 0)
            {
                animator.SetBool("Grounded", true);
                animator.SetFloat("AirSpeedY", 0);
                offGrounfDelay = 0;
            }
        }
        else
        {
            offGrounfDelay += Time.fixedDeltaTime;
            animator.SetBool("Grounded", false);
            animator.SetFloat("AirSpeedY", rb.velocity.y);
        }
    }

    private void MovementUpdate()
    {
        //MOVE
        if (state != PlayerStates.ROLLING)
        {
            if (currentSpeed != 0 && state != PlayerStates.ATTACKING)
            {
                if (state == PlayerStates.FALLING)
                {
                    rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime * settings.XSpeedModiferFalling, rb.velocity.y);
                    animator.SetInteger("AnimState", 0);
                }
                else
                {
                    animator.SetInteger("AnimState", 1);
                    rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
                }
            }
            //STOP MOVING
            else
            {
                animator.SetInteger("AnimState", 0);
                if (settings.PlayerStopsWhenKeyUp)
                {
                    if (state == PlayerStates.FALLING)
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
            }
        }
    }

    private void OnMove(float speed)
    {
        if (state != PlayerStates.DYING && state != PlayerStates.STAGGERED && state != PlayerStates.ROLLING)
        {
            if (speed != 0)
            {
                spriteRenderer.flipX = speed < 0;
            }

            currentSpeed = speed;
        }
    }

    private void OnJump()
    {
        if (state != PlayerStates.OFFGROUND && state != PlayerStates.FALLING && state != PlayerStates.ATTACKING && state != PlayerStates.DYING && state != PlayerStates.STAGGERED && state != PlayerStates.ROLLING)
        {
            rb.AddForce(new Vector2(0, settings.JumpForce));
            animator.SetTrigger("Jump");
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");
    }

    public bool TakeDamage(float dmg)
    {
        if (state == PlayerStates.DYING)
        {
            return true;
        }

        if (state == PlayerStates.ROLLING)
        {
            return false;
        }

        staggerDelay = settings.HurtStaggerDelay;

        if (staggerDelay > 0)
        {
            animator.SetTrigger("Hurt");
            currentSpeed = 0;
        }

        health -= dmg;

        if (health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
}
