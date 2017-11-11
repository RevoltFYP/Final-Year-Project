using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;

    public bool isDead { get; set; }
    private bool isSinking;

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

        if (GetComponent<SpawnAmmo>() != null)
        {
            // Spawns Ammo Box
            GetComponent<SpawnAmmo>().SpawnAmmoBox(transform.position);
        }

        if(GetComponent<HealthSpawn>() != null)
        {
            GetComponent<HealthSpawn>().SpawnHealthPack(transform.position + Vector3.forward);
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
