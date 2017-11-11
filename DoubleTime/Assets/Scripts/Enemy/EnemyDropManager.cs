using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropManager : MonoBehaviour {

    [Header("Ammo Drops")]
    public int ammoPool = 10;
    public GameObject[] ammoTypes;
    public List<GameObject> ammoDrops { get; set; }

    [Header("Health Drops")]
    public int healthPackPool = 10;
    public GameObject healthPack;
    public List<GameObject> healthDrops { get; set; }

    // Use this for initialization
    void Awake () {

        PoolHealthPacks();
        PoolAmmo();
    }

    private void PoolHealthPacks()
    {
        healthDrops = new List<GameObject>();

        for (int i = 0; i < healthPackPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(healthPack);
            healthDrops.Add(healthPack);
            obj.SetActive(false);
            GameObject.DontDestroyOnLoad(obj);
        }
    }

    private void PoolAmmo()
    {
        ammoDrops = new List<GameObject>();

        foreach (GameObject ammo in ammoTypes)
        {
            for (int i = 0; i < ammoPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(ammo);
                ammoDrops.Add(obj);
                obj.SetActive(false);
                GameObject.DontDestroyOnLoad(obj);
            }
        }
    }
}
