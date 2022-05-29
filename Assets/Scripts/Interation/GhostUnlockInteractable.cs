using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostUnlockInteractable : Interactable
{
    protected override void Interaction(PlayerGhostHandler player)
    {
        gameEvents.OnGhostUnlock();
    }
}
