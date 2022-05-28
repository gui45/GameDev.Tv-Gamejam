using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(99999, transform.position.x - player.transform.position.x);
        }
    }
}
