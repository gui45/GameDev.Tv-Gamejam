using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupButton : Button
{
    protected override void OnClick()
    {
        Destroy(GetComponentInParent<Popup>().gameObject);
    }
}
