using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

    public GameObject projectile;
    public GameObject firePoint;
    public EnemyHealth enemyHealth;
    public int damage = 5;
    private EnemyStates enemyStates;

    [Header("Projectile Stats")]
    public float timeBetweenBullet = 0.5f;
    public int projectileForceMultiplyer = 100;

    private float timer;
    private float posTimer;

    private void Awake()
    {
        enemyStates = transform.parent.GetComponent<EnemyStates>();
    }

    private void Update()
    {
        if (!enemyHealth.isDead && enemyStates.state == EnemyStates.State.AGGRO)
        {
            timer += Time.deltaTime;

            if (timer >= timeBetweenBullet)
            {
                Fire();
                timer = 0;
            }
        }
    }

    // Fires Bullet //
    void Fire()
    {
        GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);

        projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;
    }
}
