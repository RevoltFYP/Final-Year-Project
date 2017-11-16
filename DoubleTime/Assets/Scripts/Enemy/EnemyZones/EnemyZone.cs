using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour {

    [Header("Enemies")]
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Boundaries")]
    public List<GameObject> boundaries = new List<GameObject>();

    [Header("Blocker Stick Group")]
    public GameObject blockStickGroup;

    private List<GameObject> blockerSticks = new List<GameObject>();
    private GameObject player;
    private SpawnZone spawnZone;

    private void Awake()
    {
        if(blockStickGroup != null)
        {
            foreach (Transform child in blockStickGroup.transform)
            {
                blockerSticks.Add(child.gameObject);
                //child.gameObject.SetActive(false);
            }
        }

        foreach(GameObject boundary in boundaries)
        {
            boundary.SetActive(false);
        }


        if(GetComponent<SpawnZone>() != null)
        {
            spawnZone = GetComponent<SpawnZone>();
        }

        player = GameObject.FindGameObjectWithTag("Player");

        //CheckArray();
    }

    // When player damages any enemy in that zone
    private void Update()
    {
        if(enemies.Count > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    if(enemy.GetComponent<EnemyStates>().state != EnemyStates.State.AGGRO)
                    {
                        EnemyHealth enemHealth = enemy.GetComponent<EnemyHealth>();

                        if (enemHealth.currentHealth < enemHealth.startingHealth)
                        {
                            //Debug.Log("Curr hp less than starting");
                            if(spawnZone != null)
                            {
                                spawnZone.spawn = true;
                            }
                            AggroAllEnemies(player);
                        }
                    }
                }
            }
        }
    }

    // Enemies aggro when player enters zone
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ActivateBlockers(true);

            AggroAllEnemies(other.gameObject);

            if(spawnZone != null)
            {
                spawnZone.spawn = spawnZone.enemyList.Count > 0 ? true : false;
            }

            ActivateBoundaries();
        }
    }

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
        //Debug.Log(enemies.Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
        }

        if(enemies.Count == 0)
        {
            DestroyAreaZone();
        }
    }

    // Destroys zone if there are no enemies left
    protected virtual void DestroyAreaZone()
    {
        ActivateBlockers(false);
        enemies.Clear();
        RemoveBoundaries();
        Destroy(gameObject);
    }

    // Sets all enemies in list to Aggro on Target
    public void AggroAllEnemies(GameObject target)
    {
        foreach(GameObject enemy in enemies)
        {
            if(enemy != null)
            {
                enemy.GetComponent<EnemyStates>().state = EnemyStates.State.AGGRO;
            }
        }
    }

    public void ActivateBoundaries()
    {
        if (boundaries.Count > 0)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (!boundaries[i].activeSelf)
                {
                    boundaries[i].SetActive(true);
                }
            }
        }
    }

    public void RemoveBoundaries()
    {
        if (boundaries.Count > 0)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (boundaries[i].activeSelf)
                {
                    //boundaries[i].SetActive(false);
                    Destroy(boundaries[i]);
                }
            }
        }
    }

    public void ActivateBlockers(bool activate)
    {
        foreach(GameObject blocker in blockerSticks)
        {
            if(blocker != null)
            {
                blocker.GetComponent<Animator>().SetBool("activated", activate);
            }
        }
    }
}
