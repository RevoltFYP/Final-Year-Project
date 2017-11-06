using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject level;

    //public float restartDelay = 5f;
    public Image gameOverScreen;

    public Image gameWinScreen;
    public SceneManagerScript sceneManager;
    public string nextSceneName;

    private float restartTimer;
    public static GameManager instance;

    void Awake  ()
    {
        instance = this;
    }

    // Initiates game win in game //
    public void LoadNextLevel()
    {
        GameEnd();
        //gameWinScreen.gameObject.SetActive(true);
        sceneManager.SwitchScene(nextSceneName);
    }

    // Initiates game over in game //
    public void GameOver()
    {
        //Debug.Log("GameOver True");
        gameOverScreen.gameObject.SetActive(true);
        GameEnd();
        //StartCoroutine(LoadLoseScene(restartDelay));
    }

    private void GameEnd()
    {
        player.SetActive(false);
        level.SetActive(false);
    }
}
