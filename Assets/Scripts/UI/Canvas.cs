using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField]
    private UI DefaultUI;

    void Start()
    {
        Instantiate(DefaultUI, transform);
    }
}
