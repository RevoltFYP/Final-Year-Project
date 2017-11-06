using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour {

    public string ammoType;
    [Range(0,100)] public float percentageToSpawn; //percentage to spawn random weapon in arr

    // Checks against percentage to Spawn to spawn weapon at location //
    public void SpawnAmmoBox(Vector3 spawnLocation)
    {
        float percentage = Random.Range(0.0f, 100.0f);

        // If percentage to spawn is true
        if(percentage <= percentageToSpawn)
        {
            GameObject player = GameObject.Find("Player");
            WeaponInventory weapInven = player.GetComponent<WeaponInventory>();

            for (int i = 0; i < weapInven.ammoDrops.Count; i++)
            {
                if (!weapInven.ammoDrops[i].activeInHierarchy)
                {
                    if (weapInven.ammoDrops[i].name.Contains(ammoType))
                    {
                        // Spawn ammo
                        weapInven.ammoDrops[i].transform.position = spawnLocation;
                        weapInven.ammoDrops[i].transform.rotation = Quaternion.identity;
                        weapInven.ammoDrops[i].SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
