using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnZone : MonoBehaviour {

    [Header("Enemy Threshold to Spawn")]
    public int enemyThreshold;

    [Header("Total Amount")]
    public int spawnMin = 1;
    public int spawnMax = 5;
    public Vector3 size;
    private Vector3 centre;

    [Header("Total Amount")]
    public int meleeEnemy;
    public int rangeEnemy;
    public int chargingEnemy;
    public int shieldEnemy;

    [Header("Game Objects References")]
    public GameObject meleeEnemyObj;
    public GameObject rangeEnemyObj;
    public GameObject chargerEnemyObj;
    public GameObject shieldEnemyObj;

    [Header("Game Objects")]
    public EnemyZone enemyZone;

    private List<GameObject> enemyList = new List<GameObject>();
    private GameObject player;
    private int activeEnemies;

    // Use this for initialization
    void Awake () {

        centre = GetComponent<Renderer>().bounds.center;
        GetComponent<Renderer>().enabled = false;

        player = GameObject.FindGameObjectWithTag("Player");

        AddEnemyToList(meleeEnemy, meleeEnemyObj);
        AddEnemyToList(rangeEnemy, rangeEnemyObj);
        AddEnemyToList(chargingEnemy, chargerEnemyObj);
        AddEnemyToList(shieldEnemy, shieldEnemyObj);

        ShuffleList();

        InvokeRepeating("CheckActiveEnemies", 4.0f, 4.0f);
    }

    public void SpawnEnemy()
    {
        //Debug.Log("Spawn");
        //Debug.Log("EnemyList Count: " + enemyList.Count);

        // Get random amount
        int spawnAmount = Random.Range(spawnMin, spawnMax);
        Debug.Log("SpawnAmount: " + spawnAmount);

        for (int i = 0; i < spawnAmount; i ++)
        {
            if(spawnAmount < enemyList.Count)
            {
                if (enemyList[i] != null)
                {
                    // Get random pos
                    Vector3 pos = centre + new Vector3(Random.Range(-size.x / 2, size.x / 2), transform.position.y, Random.Range(-size.z / 2, size.z / 2));

                    // Set target spawn to random pos and look at player
                    enemyList[i].transform.position = pos;
                    enemyList[i].transform.rotation = Quaternion.LookRotation(player.transform.position - enemyList[i].transform.position);

                    // Set active spawn target
                    enemyList[i].SetActive(true);
                    Debug.Log("SpawnTarget Status: " + enemyList[i].activeSelf);

                    // Set Aggro State
                    enemyList[i].GetComponent<EnemyStates>().state = EnemyStates.State.AGGRO;

                    // Remove from list
                    enemyList.Remove(enemyList[i]);
                    Debug.Log(enemyList.Count);
                }

                // Re-shuffle list
                ShuffleList();
            }
            else
            {
                SpawnEnemy();
            }

        }
    }

    public void AddEnemyToList(int amount, GameObject enemy)
    {
        // If there is any enemies to add
        if(amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                // Instantiate enemy and hide it
                GameObject temp = (GameObject)Instantiate(enemy, transform.position, Quaternion.identity);
                temp.SetActive(false);

                // Set Aggro Null
                temp.GetComponent<EnemyStates>().state = EnemyStates.State.NONE;

                // Add enemy to list of enemies to spawn
                enemyList.Add(temp);

                // Add the enemy to list of enemies to kill
                enemyZone.enemies.Add(temp);
            }
        }
    }

    public void ShuffleList()
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            // store temp
            GameObject temp = enemyList[i];

            // Random an index
            int randomIndex = Random.Range(i, enemyList.Count);

            // Assign new index to element
            enemyList[i] = enemyList[randomIndex];
            enemyList[randomIndex] = temp;
        }
    }

    public void CheckActiveEnemies()
    {
        foreach (GameObject enemy in enemyZone.enemies)
        {
            if (enemy.activeInHierarchy)
            {
                activeEnemies++;
            }
        }
        Debug.Log("Active Enemies: " + activeEnemies);

        if (activeEnemies <= enemyThreshold)
        {
            SpawnEnemy();
        }

        activeEnemies = 0;
    }
}
