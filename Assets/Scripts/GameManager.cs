using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameSettings settings;
    private void Start()
    {
        settings = SettingsRepository.instance.GameSettings;

        Application.targetFrameRate = settings.FpsLimit;
    }
}
