using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Canvas))]
public class WorldSpaceCanvas : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.Canvas>().worldCamera = UnityEngine.Camera.main;
    }
}
