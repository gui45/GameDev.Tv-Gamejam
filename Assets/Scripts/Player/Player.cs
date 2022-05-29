using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
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
    private AudioSource audioSource;

    private float health;
    private float currentSpeed;
    private float offGrounfDelay;
    private float attackCoolDown;
    private bool primaryAttackFlip = false;
    private List<Enemies> enemiesInRange = new List<Enemies>();
    private PlayerStates state = PlayerStates.IDLE;
    private float staggerDelay = 0;
    private float rollDuration = 0;
    private bool blocking = false;
    private float rollCoolDown = 0;
    private bool ghost = false;

    private void Start()
    {
        feets = GetComponent<CapsuleCollider2D>();
        gameSettings = SettingsRepository.instance.GameSettings;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameEvents = GameEvents.instance;
        audioSource = GetComponent<AudioSource>();

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
        gameEvents.OnBlockEvent += OnBlock;
        gameEvents.OnGameOverEvent += OnGameOver;
        gameEvents.OnSwitchModeEvent += OnSwitchMode;
        gameEvents.OnPrimaryActionEvent += OnPrimaryAttack;
        gameEvents.OnSecondaryActionEvent += OnSecondaryActtack;
    }

    private void RemoveEvents()
    {
        gameEvents.OnRollEvent -= OnRoll;
        gameEvents.onJumpEvent -= OnJump;
        gameEvents.onMoveEvent -= OnMove;
        gameEvents.OnBlockEvent -= OnBlock;
        gameEvents.OnGameOverEvent -= OnGameOver;
        gameEvents.OnSwitchModeEvent -= OnSwitchMode;
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
            EvalState();
            if (state != PlayerStates.GHOST)
            {
                HandeHurtStagger();
                HandleAttackCoolDown();
                CheckOnGround();
                HandleBlocking();
            }
        }

        if (state != PlayerStates.MOVING && audioSource.clip == settings.MoveSound)
        {
            StopClip();
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

    private void OnGameOver()
    {
        Destroy(this);
    }

    private void EvalState()
    {

        if (health <= 0 )
        {
            state = PlayerStates.DYING;
        }
        else if (ghost)
        {
            state = PlayerStates.GHOST;
        }
        else if (staggerDelay > 0)
        {
            rollDuration = 0;
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
        else if (blocking)
        {
            currentSpeed = 0;
            state = PlayerStates.BLOCKING;
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

    private void OnSwitchMode()
    {
        ghost = !ghost;

        if (ghost)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("Revive");
        }
    }

    private void HandleBlocking()
    {
        if (blocking && state == PlayerStates.BLOCKING)
        {
            if (!animator.GetBool("IdleBlock"))
            {
                animator.SetBool("IdleBlock", true);
                animator.SetTrigger("Block");

                if (blocking)
                {
                    PlayClip(settings.BlockedSound);
                }
            }
        }
        else if(state != PlayerStates.BLOCKING)
        {
            if (animator.GetBool("IdleBlock"))
            {
                animator.SetBool("IdleBlock", false);
            }
        }
    }

    private void OnBlock()
    {
        blocking = !blocking;
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

            if (rollDuration <= 0)
            {
                rollDuration = 0;

                feets.offset = new Vector2(feets.offset.x, feets.offset.y * 2);
                feets.size = new Vector2(feets.size.x, feets.size.y * 2);

                gameObject.layer = LayerMask.NameToLayer(gameSettings.PlayerLayer);

                rollCoolDown = settings.RollCoolDown;
            }
        }

        if (rollCoolDown > 0)
        {
            rollCoolDown -= Time.deltaTime;
        }else
        {
            rollCoolDown = 0;
        }
    }

    private void OnRoll()
    {
        if (state != PlayerStates.GHOST && state != PlayerStates.ROLLING && rollCoolDown <= 0 && state != PlayerStates.BLOCKING && state != PlayerStates.STAGGERED && state != PlayerStates.FALLING && state != PlayerStates.DYING && state != PlayerStates.ATTACKING)
        {
            animator.SetTrigger("Roll");
            feets.offset = new Vector2(feets.offset.x, feets.offset.y / 2);
            feets.size = new Vector2(feets.size.x, feets.size.y / 2);

            rollDuration = settings.RollDuration;

            gameObject.layer = LayerMask.NameToLayer(gameSettings.PlayerInvulnLayer);

            float xForce = spriteRenderer.flipX ? -settings.RollForce : settings.RollForce;
            rb.AddForce(new Vector2(xForce, 0));
            PlayClip(settings.RollSound);
        }
    }

    private void OnPrimaryAttack()
    {
        if (state != PlayerStates.GHOST && attackCoolDown <= 0 && state != PlayerStates.BLOCKING && state != PlayerStates.STAGGERED && state != PlayerStates.DYING && state != PlayerStates.ROLLING)
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
            PlayClip(settings.PrimaryAttackSound);
        }
    }

    private void OnSecondaryActtack()
    {
        if (state != PlayerStates.GHOST && attackCoolDown <= 0 && state != PlayerStates.BLOCKING && state != PlayerStates.STAGGERED && state != PlayerStates.DYING && state != PlayerStates.ROLLING)
        {
            animator.SetTrigger("Attack3");
            attackCoolDown += settings.SecondaryAttackCoolDown;
            CauseDamage(settings.SecondaryAttackDamage);
            PlayClip(settings.SecodaryAttackSound);
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
                PlayClip(settings.LandingSound);
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
        if (state != PlayerStates.GHOST && state != PlayerStates.ROLLING && state != PlayerStates.BLOCKING)
        {
            if (currentSpeed != 0 && state != PlayerStates.ATTACKING)
            {
                spriteRenderer.flipX = currentSpeed < 0;
                if (state == PlayerStates.FALLING)
                {

                    rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime * settings.XSpeedModiferFalling, rb.velocity.y);
                    animator.SetInteger("AnimState", 0);
                }
                else
                {
                    LoopClip(settings.MoveSound);
                    animator.SetInteger("AnimState", 1);
                    rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
                }
            }
            //STOP MOVING
            else
            {
                if (audioSource.clip == settings.MoveSound)
                {
                    StopClip();
                }
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
        else
        {
            if (audioSource.clip == settings.MoveSound)
            {
                StopClip();
            }
        }
    }

    private void OnMove(float speed)
    {
        currentSpeed = speed; // toujours set la vitesse pour prevenir les controles qui cole
    }

    private void OnJump()
    {
        if (state != PlayerStates.GHOST && state != PlayerStates.BLOCKING && state != PlayerStates.OFFGROUND && state != PlayerStates.FALLING && state != PlayerStates.ATTACKING && state != PlayerStates.DYING && state != PlayerStates.STAGGERED && state != PlayerStates.ROLLING)
        {
            rb.AddForce(new Vector2(0, settings.JumpForce));
            animator.SetTrigger("Jump");
            PlayClip(settings.JumpSound);
        }
    }

    private void Die()
    {
        gameObject.layer = LayerMask.NameToLayer(gameSettings.PlayerInvulnLayer);
        audioSource.pitch = 1;
        PlayClip(settings.dieSound);
        gameEvents.OnGameOver();
        animator.SetTrigger("Death");
    }

    public bool TakeDamage(float dmg, float xIncomingDirection)
    {
        switch (state)
        {
            case PlayerStates.DYING:
                return true;
            case PlayerStates.ROLLING:
                return false;
            case PlayerStates.BLOCKING:
                if ((xIncomingDirection > 0 && !spriteRenderer.flipX) || (xIncomingDirection <= 0 && spriteRenderer.flipX))
                {
                    return BlockingIncomingDamage(dmg, xIncomingDirection);
                }
                else
                {
                    return DefaultIncomingDamage(dmg);
                }

            default:
                return DefaultIncomingDamage(dmg);
        }
    }

    private bool BlockingIncomingDamage(float dmg, float xIncomingDirection)
    {
        float force = xIncomingDirection > 0 ? dmg * -settings.PushBackForce : dmg * settings.PushBackForce;

        float finalDamage = dmg * (1 - settings.BlockedDamage);

        health -= finalDamage;

        rb.AddForce(new Vector2(force, 0));


        if (health <= 0)
        {
            gameEvents.OnPlayerHealthChange(0, settings.Health);
            Die();
            return true;
        }
        else
        {
            gameEvents.OnPlayerHealthChange(health, settings.Health);
            PlayClip(settings.BlockedSound);
            animator.SetTrigger("Block");
            return false;
        }
    }

    private bool DefaultIncomingDamage(float dmg)
    {
        staggerDelay = settings.HurtStaggerDelay;

        if (staggerDelay > 0)
        {
            animator.SetTrigger("Hurt");
            currentSpeed = 0;
        }

        health -= dmg;


        if (health <= 0)
        {
            gameEvents.OnPlayerHealthChange(0, settings.Health);
            Die();
            return true;
        }

        gameEvents.OnPlayerHealthChange(health, settings.Health);

        PlayClip(settings.HurtSound);

        return false;
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void LoopClip(AudioClip clip)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopClip()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
    }
}
