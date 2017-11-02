using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    [Header("Bullet Stats")]
    public float destroyIn = 5;
    public int projectileDamage { get; set; }

    [Header("Misc")]
    public string damageTag;
    public ParticleSystem damageParticles;

    //[Header("Ignore SlowTime")]
    public int speed = 10;
    //public bool ignoreTimeSlow = false;

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Invoke("Destroy", destroyIn);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Just Damages //
    void OnTriggerEnter (Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag.Contains(damageTag))
        {
            //Debug.Log(other.gameObject.name);
            // If playerHealth exists on Target, damages playerHealth //
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth.currentHealth > 0)
                {
                    playerHealth.TakeDamage(projectileDamage);
                }
            }
            // else if enemyHealth exists on Target, damages enemyHealth instead //
            else if (other.gameObject.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                if (enemyHealth.currentHealth > 0)
                {
                    enemyHealth.TakeDamage(projectileDamage);
                }
            }
            Destroy();
            SpawnParticles(damageParticles, transform.position, transform.rotation, false);
        }
        else if (other.gameObject.layer == 11)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    // Instantiates Particles at location(Vector3) with Quaternion Identity //
    public void SpawnParticles(ParticleSystem particles, Vector3 location, Quaternion rotation, bool playParticles)
    {
        Instantiate(particles, location, rotation);

        if (playParticles)
        {
            particles.Play();
        }
    }
}
