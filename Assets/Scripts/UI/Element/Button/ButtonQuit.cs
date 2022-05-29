using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuit : Button
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}
