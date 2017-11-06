using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider loadingBar;
    public Text loadingPercent;

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        if(sceneName != null)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                loadingBar.value = progress;
                loadingPercent.text = string.Format("{0:0}%", progress * 100f);

                yield return null;
            }
        }
        else
        {
            Debug.LogError("No Scene to load or scene name written wrongly");
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
