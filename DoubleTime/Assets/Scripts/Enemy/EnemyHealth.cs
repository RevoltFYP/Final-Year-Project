using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpawnAmmo))]
[RequireComponent(typeof(SpawnScrap))]

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;

    public bool isDead { get; set; }
    private bool isSinking;

    [Header("Upon Death")]
    public bool spawnAmmo;
    public bool spawnScrap;

    void Awake()
    {
        currentHealth = startingHealth;
        isDead = false;
    }

    void Update()
    {
        if(isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        //Debug.Log(hitParticles.isPlaying);
    }

    // Minus enemy health //
    public void TakeDamage (int amount)
    {
        //Debug.Log("Damage Taken: " + amount);
        if (isDead)
            return;
        currentHealth -= amount;
 
        if (currentHealth <= 0)
        {
            Death();
            StartSinking();
        }
    }

    // When enemy Dies //
    void Death ()
    {
        isDead = true;

        if (spawnAmmo)
        {
            // Spawns Ammo Box
            SpawnAmmo spawnAmmoScript = GetComponent<SpawnAmmo>();
            spawnAmmoScript.SpawnAmmoBox(transform.position);
        }

        if (spawnScrap)
        {
            // Spawns Scrap
            SpawnScrap spawnScrapScript = GetComponent<SpawnScrap>();
            spawnScrapScript.SpawnScrapObject(transform.position + transform.forward);
        }

        // Destroy all scripts in child and object except EnemyHealth
        List<MonoBehaviour> scripts = new List<MonoBehaviour>();

        foreach (Transform child in transform)
        {
            scripts.Add(child.GetComponent<MonoBehaviour>());
        }

        for (int i = 0; i < scripts.Count; i++)
        {
            if (scripts[i] != GetComponent<EnemyHealth>())
            {
                Destroy(scripts[i]);
            }
        }

        GetComponent<Collider>().enabled = false;
    }

    // Enemy starts sinking (disables unnecessary components)//
    public void StartSinking ()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }

}
