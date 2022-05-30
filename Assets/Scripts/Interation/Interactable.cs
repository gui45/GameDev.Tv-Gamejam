using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool OneTime;
    [SerializeField]
    private bool CanUseByGhost = true;
    protected GameEvents gameEvents;
    protected GameSettings gameSettings;
    private bool playerInside = false;
    PlayerGhostHandler player;
    private GameObject spawnedClue;
    private Animator animator;

    protected void Start()
    {
        gameSettings = SettingsRepository.instance.GameSettings;
        gameEvents = GameEvents.instance;
        animator = GetComponent<Animator>();

        AddEvents();
    }

    private void AddEvents()
    {
        gameEvents.OnInteractEvent += OnInteract;
    }

    private void RemoveEvents()
    {
        gameEvents.OnInteractEvent -= OnInteract;
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.GetComponentInParent<PlayerGhostHandler>() && CanUseByGhost) ||
            collision.GetComponent<Player>())
        {
            if (!spawnedClue)
            {
                spawnedClue = Instantiate(gameSettings.InteractableClue, transform.position + new Vector3(0, 2, 0), transform.rotation, transform);
            }
            player = collision.GetComponentInParent<PlayerGhostHandler>();
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerGhostHandler>())
        {
            if (!spawnedClue)
            {
                Destroy(spawnedClue);
            }
            playerInside = false;
        }
    }

    private void OnInteract()
    {
        if (playerInside)
        {
            Interaction(player);

            if (animator != null)
            {
                animator.SetTrigger("Interact");
            }

            if (OneTime)
            {
                Destroy(spawnedClue);
                Destroy(this);
            }
        }
    }

    protected abstract void Interaction(PlayerGhostHandler player);
}
