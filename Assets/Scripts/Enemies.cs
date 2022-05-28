using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Enemies : MonoBehaviour
{
    [SerializeField]
    private EnemiesSettings settings;
    private Rigidbody2D rb;
    private Animator animator;
    private GameEvents gameEvents;
    private GameSettings gameSettings;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;


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
    private bool isDead;

    [SerializeField]
    LayerMask[] layerMasksFlip;

    [SerializeField]
    LayerMask newLayerOnDeath;

    [SerializeField]
    GameObject[] gameObjectsToDesactiveOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = SettingsRepository.instance.GameSettings;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        health = settings.Health;
        isAttacking = false;
        canAttack = true;
        isDead = false;
        animator.SetBool("IsDead", false);

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
        if (isDead)
            return;

        if(DetectPlayerAttack1() && canAttack)
        {
            StopClip();
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
            if (playerInRange.TakeDamage(damage, 1 ))
            {
                playerInRange = null;
            }
        }
        else if (!spriteRenderer.flipX && playerInRange.transform.position.x > transform.position.x)
        {
            if (playerInRange.TakeDamage(damage, -1 ))
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
            PlayClip(settings.PrimaryAttackSound);
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
        LoopClip(settings.MoveSound);

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

        animator.SetTrigger("TakeDamage");
        animator.SetBool("IsAttacking", false);
        isAttacking = false;

        if (health <= 0)
        {
            Die();
            return true;
        }

        PlayClip(settings.HurtSound);
        return false;
    }
    private void Die()
    {
        StopClip();
        isDead = true;
        PlayClip(settings.dieSound);
        animator.SetTrigger("Die");

        gameObject.layer = LayerMask.NameToLayer(gameSettings.DeadBodyLayer);

        foreach(var obj in gameObjectsToDesactiveOnDeath)
        {
            obj.SetActive(false);
        }
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
