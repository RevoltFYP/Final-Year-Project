using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnZone : MonoBehaviour {

    [Header("Spawn Settings")]
    public int enemyThreshold;
    private float checkRate = 4.0f;
    public bool spawn;

    [Header("Spawn Points")]
    public List<GameObject> spawnPoints = new List<GameObject>();
    private int minIndex;
    private int maxIndex;
    private GameObject chosenSpawn;

    [Header("Spawn Amount")]
    public int spawnMin = 1;
    public int spawnMax = 5;
    private Vector3 centre;
    private GameObject player;
    private int activeEnemies;

    [Header("Total Amount")]
    public int meleeEnemy;
    public int rangeEnemy;
    public int chargingEnemy;
    public int shieldEnemy;
    private List<GameObject> enemyList = new List<GameObject>();

    [Header("Game Objects References")]
    public GameObject meleeEnemyObj;
    public GameObject rangeEnemyObj;
    public GameObject chargerEnemyObj;
    public GameObject shieldEnemyObj;

    [Header("Corresponding Enemy Zone")]
    public EnemyZone enemyZone;

    // Use this for initialization
    void Awake () {

        // Hide all renderers of spawn points
        foreach(GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.GetComponent<Renderer>().enabled)
            {
                spawnPoint.GetComponent<Renderer>().enabled = false;
            }
        }

        minIndex = 0;
        maxIndex = spawnPoints.Count;

        player = GameObject.FindGameObjectWithTag("Player");

        // Pool enemies
        AddEnemyToList(meleeEnemy, meleeEnemyObj);
        AddEnemyToList(rangeEnemy, rangeEnemyObj);
        AddEnemyToList(chargingEnemy, chargerEnemyObj);
        AddEnemyToList(shieldEnemy, shieldEnemyObj);

        // Randomize list of enemies to spawn
        ShuffleList();

        // Check for spawning every 
        InvokeRepeating("CheckActiveEnemies", checkRate, checkRate);
    }

    private void Update()
    {
        if(enemyList.Count <= 0)
        {
            CancelInvoke();
        }
    }

    public void PickSpawnPoint()
    {
        // More than 1 spawn point
        if(spawnPoints.Count > 1)
        {
            // Randomize spawn point
            int pickIndex = Random.Range(minIndex, maxIndex);
            chosenSpawn = spawnPoints[pickIndex];
        }
        else
        {
            // Pick the only spawn point
            chosenSpawn = spawnPoints[minIndex];
        }
    }

    // Spawns enemy within a random pos in spawn point
    public void SpawnEnemy()
    {
        //Debug.Log("Spawn");
        //Debug.Log("EnemyList Count: " + enemyList.Count);

        centre = chosenSpawn.GetComponent<Renderer>().bounds.center;
        Vector3 size = chosenSpawn.transform.localScale;
        //chosenSpawn = null;

        // Get random amount
        int spawnAmount = Random.Range(spawnMin, spawnMax);

        Debug.Log(spawnAmount);

        //Debug.Log("SpawnAmount: " + spawnAmount);

        if (spawnAmount <= enemyList.Count)
        {
            for (int i = 0; i < spawnAmount; i++)
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
                    // Debug.Log("SpawnTarget Status: " + enemyList[i].activeSelf);

                    // Set Aggro State
                    enemyList[i].GetComponent<EnemyStates>().state = EnemyStates.State.AGGRO;

                    // Remove from list
                    enemyList.Remove(enemyList[i]);
                    //Debug.Log(enemyList.Count);

                    // Re-shuffle list
                    ShuffleList();
                }
            }
        }
        else
        {
            Debug.Log("True");

            spawnAmount = enemyList.Count;

            // Re-run until spawn amount is less than max amount 
            SpawnEnemy();
        }
    }

    // Pool enemies to spawn
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

    // Shuffle List to have random spawning
    public void ShuffleList()
    {
        if(enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
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
    }

    public void CheckActiveEnemies()
    {
        if (spawn)
        {
            if (enemyZone.enemies.Count > 0)
            {
                foreach (GameObject enemy in enemyZone.enemies)
                {
                    if (enemy.activeInHierarchy)
                    {
                        activeEnemies++;
                    }
                }
            }

            //Debug.Log("Active Enemies: " + activeEnemies);

            if (activeEnemies <= enemyThreshold)
            {
                // Picks which spawnpoint among the list to spawn
                PickSpawnPoint();

                // Spawn enemy at point
                SpawnEnemy();
            }

            activeEnemies = 0;
        }
    }
}
