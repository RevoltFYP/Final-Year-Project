using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScrap : MonoBehaviour {

    [Range(0, 100)] public float percentageToSpawn; //percentage to spawn random weapon in arr

    // Checks against percentage to Spawn to spawn weapon at location //
    public void SpawnScrapObject(Vector3 spawnLocation)
    {
        float percentage = Random.Range(0.0f, 100.0f);

        // If percentage to spawn is true
        if (percentage <= percentageToSpawn)
        {
            GameObject player = GameObject.Find("Player");
            ScrapManager scrapManager = player.GetComponent<ScrapManager>();

            for (int i = 0; i < scrapManager.scrapList.Count; i++)
            {
                if (!scrapManager.scrapList[i].activeInHierarchy)
                {
                    // Spawn ammo
                    scrapManager.scrapList[i].transform.position = spawnLocation;
                    scrapManager.scrapList[i].transform.rotation = Quaternion.identity;
                    scrapManager.scrapList[i].SetActive(true);
                    break;
                }
            }
        }
    }
}
