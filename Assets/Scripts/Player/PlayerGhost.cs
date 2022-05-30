using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerGhost : MonoBehaviour
{
    public GhostSettings settings;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameEvents gameEvents;
    private CapsuleCollider2D capsuleCollider;
    private GameSettings gameSettings;

    private PlayerGhostStates state;
    private float health;
    private float currentSpeed;
    private bool ghost = false;
    private float offGrounfDelay;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        gameSettings = SettingsRepository.instance.GameSettings;
        gameEvents = GameEvents.instance;

        AddEvents();

        health = settings.Health;
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        EvalState();
        CheckOnGround();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        gameEvents.onMoveEvent += OnMove;
        gameEvents.onJumpEvent += OnJump;
        gameEvents.OnGameOverEvent += OnGameOver;
        gameEvents.OnSwitchModeEvent += OnSwitchMode;
    }

    private void RemoveEvents()
    {
        gameEvents.onMoveEvent -= OnMove;
        gameEvents.onJumpEvent -= OnJump;
        gameEvents.OnGameOverEvent -= OnGameOver;
        gameEvents.OnSwitchModeEvent -= OnSwitchMode;
    }

    private void OnGameOver()
    {
        Destroy(this);
    }

    private void EvalState()
    {

        if (health <= 0)
        {
            state = PlayerGhostStates.DYING;
        }
        else if (!ghost)
        {
            state = PlayerGhostStates.NOTGHOST;
        }
        else if (offGrounfDelay > 0)
        {
            if (offGrounfDelay > settings.OffGroundDelayToFall)
            {
                state = PlayerGhostStates.FALLING;
            }
            else
            {
                state = PlayerGhostStates.OFFGROUND;
            }
        }
        else if (currentSpeed != 0)
        {
            state = PlayerGhostStates.MOVING;
        }
        else
        {
            state = PlayerGhostStates.IDLE;
        }
    }

    private void CheckOnGround()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask(gameSettings.GroundLayer)))
        {
            if (offGrounfDelay > 0)
            {
                offGrounfDelay = 0;
            }
        }
        else
        {
            offGrounfDelay += Time.fixedDeltaTime;
        }
    }

    private void MovementUpdate()
    {
        //MOVE
        if (currentSpeed != 0)
        {
            spriteRenderer.flipX = currentSpeed > 0;
            if (state == PlayerGhostStates.FALLING)
            {
                rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime * settings.XSpeedModiferFalling, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.fixedDeltaTime, rb.velocity.y);
            }
        }
        //STOP MOVING
        else
        {
            if (settings.PlayerStopsWhenKeyUp)
            {
                if (state == PlayerGhostStates.FALLING)
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

    private void OnSwitchMode()
    {
        ghost = !ghost;

        if (ghost)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            transform.position = new Vector2(-100, -100); // throwing it away as a quick fix
        }
    }

    private void OnMove(float speed)
    {
        currentSpeed = speed; // toujours set la vitesse pour prevenir les controles qui cole
    }

    private void OnJump()
    {
        if (state != PlayerGhostStates.OFFGROUND && state != PlayerGhostStates.FALLING && state != PlayerGhostStates.DYING)
        {
            rb.AddForce(new Vector2(0, settings.JumpForce));
        }
    }

    public bool TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            gameEvents.OnPlayerGhostHealthChange(0, settings.Health);
            return true;
        }
        else
        {
            gameEvents.OnPlayerGhostHealthChange(health, settings.Health);
            return false;
        }
    }
}
