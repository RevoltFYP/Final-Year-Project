using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour {

    public List<GameObject> markedEnemies = new List<GameObject>();

    public string changeToScene;

    public SceneManagerScript sceneManager;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CheckArray();
        }
    }

    // Checks if there are any enemies left in zone
    private void CheckArray()
    {
        for (int i = 0; i < markedEnemies.Count; i++)
        {
            if (markedEnemies[i] == null)
            {
                markedEnemies.Remove(markedEnemies[i]);
            }
        }
        DestroyAreaZone();
    }

    // Destroys zone if there are no enemies left
    private void DestroyAreaZone()
    {
        if (markedEnemies.Count == 0)
        {
            // Clear List
            markedEnemies.Clear();

            // Destroy this game object
            Destroy(gameObject);

            // Initiates game win sequence
            //GameManager.instance.LoadNextLevel();
            sceneManager.SwitchScene(changeToScene);
        }

        Debug.Log("Enemies: " + markedEnemies.Count);
    }
}
