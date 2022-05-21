using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsRepository : MonoBehaviour
{
    public static SettingsRepository instance { get; private set; }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Game Event instance is already set");
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    private GameSettings gameSettings;
    public GameSettings GameSettings => gameSettings;
}
