using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : MonoBehaviour {

    // Temporarily for testing loading Screen //
    public KeyCode pauseKey;
    public GameObject pauseScreen;
    public GameObject settingsButton;
    private bool pauseScreenDisplay;

    public MonoBehaviour[] disableScripts;

    private void Awake()
    {
        settingsButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        settingsButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {

        // Temporarily for testing loading Screen //
        if (Input.GetKeyDown(pauseKey))
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseScreenDisplay = !pauseScreenDisplay;
        pauseScreen.SetActive(pauseScreenDisplay);

        foreach (MonoBehaviour script in disableScripts)
        {
            script.enabled = !pauseScreenDisplay;
        }

        Time.timeScale = pauseScreenDisplay ? 0 : 1;
    }
}
