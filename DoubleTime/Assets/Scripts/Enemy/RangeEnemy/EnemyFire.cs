using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

    public GameObject projectile;
    public GameObject firePoint;
    public EnemyHealth enemyHealth;
    public EnemyStates enemyStates;
    public int damage = 5;

    [Header("Projectile Stats")]
    public float initialDelayTime;
    public float timeBetweenBullet = 0.5f;
    public int projectileForceMultiplyer = 100;

    private float timer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemyStates.StopAgent(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!enemyHealth.isDead)
            {
                timer += Time.deltaTime;

                if (timer >= timeBetweenBullet)
                {
                    Fire();
                    timer = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemyStates.StopAgent(false);
        }
    }

    // Fires Bullet //
    void Fire()
    {
        GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);

        projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;

        //Invoke("Fire", timeBetweenBullet);
    }
}
