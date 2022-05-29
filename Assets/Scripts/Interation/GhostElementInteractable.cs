using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostElementInteractable : Interactable
{
    [SerializeField]
    private GameObject makeReal;
    [SerializeField]
    private GameObject clue;

    protected override void Interaction(PlayerGhostHandler player)
    {
        makeReal.SetActive(true);
        makeReal.transform.parent = null;
        if (makeReal.GetComponent<SpriteRenderer>() != null)
        {
            makeReal.GetComponent<SpriteRenderer>().color = Color.white;
        }
        makeReal.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => x.color = Color.white);

        Destroy(clue);
    }
}
