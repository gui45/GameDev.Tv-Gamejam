using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangePage : Button
{
    [SerializeField]
    private UI nextUI;

    protected override void OnClick()
    {
        gameEvents.OnNextUI(nextUI);
    }
}
