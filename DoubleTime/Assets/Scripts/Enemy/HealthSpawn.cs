using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour {

    [Range(0, 100)] public float percentageToSpawn; //percentage to spawn health

    private GameObject enemyDropManager;
    private EnemyDropManager managerScript;
    // Use this for initialization
    void Awake () {

        enemyDropManager = GameObject.Find("EnemyDropManager");
        managerScript = enemyDropManager.GetComponent<EnemyDropManager>();
	}

    public void SpawnHealthPack(Vector3 spawnLocation)
    {
        //Debug.Log("Spawned Health");
        float percentage = Random.Range(0.0f, 100.0f);

        if (percentage <= percentageToSpawn)
        {
            // temp
            //Instantiate(managerScript.healthPack, spawnLocation, Quaternion.identity);

            //Debug.Log("Lower Percentage");
            for (int i = 0; i < managerScript.healthDrops.Count; i++)
            {
                //Debug.Log("Looping");

                if (!managerScript.healthDrops[i].activeInHierarchy)
                {
                    //Debug.Log("Health Pack Available");

                    managerScript.healthDrops[i].transform.position = spawnLocation;
                    managerScript.healthDrops[i].transform.rotation = Quaternion.Euler(0,0,30);
                    managerScript.healthDrops[i].SetActive(true);
                    break;
                }

            }
        }

    }
}
