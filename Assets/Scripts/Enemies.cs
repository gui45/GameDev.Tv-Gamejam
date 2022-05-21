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


    private float health;
    private float currentSpeed;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = SettingsRepository.instance.GameSettings;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        health = settings.Health;
        currentSpeed = settings.MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        //animator.SetInteger("AnimState", 1);
        rb.velocity = new Vector2(currentSpeed * settings.MovementSpeed * Time.deltaTime, rb.velocity.y);
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
