using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeScene : Button
{
    public Scenes scene;

    protected override void OnClick()
    {
        gameEvents.OnNextScene((int)scene);
    }
}
