using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour {

    public GameObject ammoType;
    [Range(0,100)] public float percentageToSpawn; //percentage to spawn random weapon in arr

    private GameObject enemyDropManager;
    private EnemyDropManager managerScript;

    private void Awake()
    {
        enemyDropManager = GameObject.Find("EnemyDropManager");
        managerScript = enemyDropManager.GetComponent<EnemyDropManager>();
    }

    // Checks against percentage to Spawn to spawn weapon at location //
    public void SpawnAmmoBox(Vector3 spawnLocation)
    {
        //Debug.Log("Spawned Ammo");
        float percentage = Random.Range(0.0f, 100.0f);

        // If percentage to spawn is true
        if(percentage <= percentageToSpawn)
        {
            for (int i = 0; i < managerScript.ammoDrops.Count; i++)
            {
                if (!managerScript.ammoDrops[i].activeInHierarchy)
                {
                    if (managerScript.ammoDrops[i].name == ammoType.name + "(Clone)")
                    {
                        //Debug.Log("Matched Ammo Type");
                        // Spawn ammo
                        managerScript.ammoDrops[i].transform.position = spawnLocation;
                        managerScript.ammoDrops[i].transform.rotation = Quaternion.identity;
                        managerScript.ammoDrops[i].SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
