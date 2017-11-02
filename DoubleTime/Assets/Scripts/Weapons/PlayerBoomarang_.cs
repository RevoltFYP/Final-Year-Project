using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoomarang_ : MonoBehaviour {

    //public float travelDistance = 20f;
    public float travelTime = 0.5f;
    public float travelSpeed = 20;
    public int damagePerAttack = 20;
    public ParticleSystem damageParticles;

    private Transform playerTransform;
    //private float Distance;
    private float travelTimer;
    private bool returning = false;
    private EnemyHealth enemyHealth;

    // Use this for initialization
    void Awake()
    {
        travelTimer = 0f;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        travelTimer += Time.deltaTime;
        //Debug.Log(travelTimer);

        //if (Distance >= travelDistance)
        if (travelTimer >= travelTime)
        {
            returning = true;
        }

        if (!returning)
        {
            transform.Translate(Vector3.forward * travelSpeed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(playerTransform.position);
            transform.Translate(Vector3.forward * travelSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("DESTROY");
            //ShootBoomarang.haveBoomarang = true;
            Destroy(this.gameObject);

        }
        if (other.gameObject.tag == "Wall")
        {
            //Debug.Log("Hit_WALL");
            returning = true;
        }
        if (other.gameObject.tag.Contains("Enemy"))
        {
            enemyHealth = other.GetComponent<EnemyHealth>();

            if(enemyHealth.currentHealth > 0)
            {
                enemyHealth.TakeDamage(damagePerAttack);
                SpawnParticles(damageParticles, transform.position, transform.rotation, false);
            }
            //Debug.Log("HIT_Melee");
            //returning = true;
        }

    }


    public void SpawnParticles(ParticleSystem particles, Vector3 location, Quaternion rotation, bool playParticles)
    {
        Instantiate(particles, location, rotation);

        if (playParticles)
        {
            particles.Play();
        }
    }
}
