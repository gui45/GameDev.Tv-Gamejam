using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float timeAsGhost;
    private bool ghost;
    private GameEvents gameEvents;
    public bool ghostUnlocked = false;

    private Popup escPopup;
    private GameSettings settings;
    private void Start()
    {
        gameEvents = GameEvents.instance;
        settings = SettingsRepository.instance.GameSettings;

        Application.targetFrameRate = settings.FpsLimit;

        DontDestroyOnLoad(gameObject);

        AddEvents();

        //eww
        settings.MainMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1)) * 20);
        settings.MainMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20);
        settings.MainMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1)) * 20);
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void Update()
    {
        if (ghost)
        {
            CountDownTimeAsGhost();
        }
    }

    private void AddEvents()
    {
        gameEvents.OnSwitchModeEvent += OnSwitchGhost;
        gameEvents.OnNextSceneEvent += OnNextScene;
        gameEvents.OnUnlockGhostEvent += OnUnlockGhost;
    }

    private void RemoveEvents()
    {
        gameEvents.OnSwitchModeEvent -= OnSwitchGhost;
        gameEvents.OnNextSceneEvent -= OnNextScene;
        gameEvents.OnUnlockGhostEvent -= OnUnlockGhost;
    }

    private void OnUnlockGhost()
    {
        ghostUnlocked = true;
    }

    private void OnNextScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    private void OnSwitchGhost()
    {
        ghost = !ghost;
        if (ghost)
        {
            timeAsGhost = 0;
        }
    }

    private void CountDownTimeAsGhost()
    {
        timeAsGhost += Time.deltaTime;

        if (timeAsGhost >= settings.MaxTimeAsGhost)
        {
            gameEvents.OnSwitchMode();
        }
    }

    public bool CanOpenEscPopup()
    {
        return SceneManager.GetActiveScene().buildIndex != 0;//not main menu
    }

    //...
    public void OnEsc()
    {
        if (GetComponent<GameManager>().CanOpenEscPopup())
        {
            if (escPopup == null)
            {
                escPopup = Instantiate(settings.EscPopup, FindObjectOfType<Canvas>().transform);
            }
            else
            {
                Destroy(escPopup.gameObject);
            }
        }
    }
}
