using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeventEndInteractable : Interactable
{
    [SerializeField]
    private Scenes sceneToLoad;

    protected override void Interaction(PlayerGhostHandler player)
    {
        gameEvents.OnNextScene((int)sceneToLoad);
    }
}
