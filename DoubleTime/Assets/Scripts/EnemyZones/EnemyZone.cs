using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour {

    public List<GameObject> enemies = new List<GameObject>();

    private GameObject player;

    private SpawnZone spawnZone;

    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        CheckArray();
    }

    // When player damages any enemy in that zone
    private void Update()
    {
        CheckArray();

        if(enemies.Count > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                EnemyHealth enemHealth = enemy.GetComponent<EnemyHealth>();

                if (enemHealth.currentHealth < enemHealth.startingHealth)
                {
                    //Debug.Log("Curr hp less than starting");
                    AggroAllEnemies(player);
                }
            }
        }
    }

    // Enemies aggro when player enters zone
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            AggroAllEnemies(other.gameObject);

            if(GetComponent<SpawnZone>() != null)
            {
                GetComponent<SpawnZone>().spawn = true;
            }
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
        enemies.Clear();
        Destroy(gameObject);
    }

    // Sets all enemies in list to Aggro on Target
    private void AggroAllEnemies(GameObject target)
    {
        foreach(GameObject enemy in enemies)
        {
            if(enemy != null)
            {
                enemy.GetComponent<EnemyStates>().state = EnemyStates.State.AGGRO;
            }
        }
    }


}
