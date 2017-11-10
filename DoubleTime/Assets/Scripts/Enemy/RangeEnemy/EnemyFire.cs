using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

    public enum Mode
    {
        REGULAR,
        BURST,
        SPREAD,
        REGULARWITHGRENADE
    }

    public Mode mode;

    public GameObject grenadePrefab;
    public GameObject projectile;
    public GameObject firePoint;
    public EnemyHealth enemyHealth;
    public int damage = 5;
    private EnemyStates enemyStates;

    [Header("Projectile Stats")]
    public float timeBetweenBullets = 1.0f;
    public int projectileForceMultiplyer = 100;

    [Header("Burst Settings")]
    public float burstTime;
    public int projectilesPerBurst = 5;

    [Header("Spread Settings")]
    public float spreadTime;
    public int spreadSize = 5;
    public float spread;

    [Header("Grenade Settings")]
    public float throwForce = 10f;
    public float timeBetweenGrenade = 5f;
    public float waitTime = 1f;

    private bool throwing = false;
    private bool hasThrown = false;

    private float gTimer;
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
            switch (mode)
            {
                case EnemyFire.Mode.REGULAR:
                    RegularFire();
                    break;
                case EnemyFire.Mode.BURST:
                    StartCoroutine(BurstFire());
                    break;
                case EnemyFire.Mode.SPREAD:
                    SpreadShot();
                    break;
                case EnemyFire.Mode.REGULARWITHGRENADE:
                    ThrowGrenade();
                    break;
            }
        }
    }

    private void SpreadShot()
    {
        timer += Time.deltaTime;

        float distanceBetweenBullets = ((spread - (-spread) / spreadSize));

        if(timer >= spreadTime)
        {
            float angleChange = 0;

            for(int i = 0; i < spreadSize; i++)
            {
                Quaternion fireRotation = Quaternion.LookRotation(transform.forward); // converts transform forward into Quaternion
                Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, (-spread + angleChange), 0)); // set rotation of each bullet

                GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, fireRotation * bulletRotation);
                projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;

                angleChange += distanceBetweenBullets;
            }

            angleChange = 0;
            timer = 0;
        }
    }

    private IEnumerator BurstFire()
    {
        timer += Time.deltaTime;
        
        if(timer >= burstTime)
        {
            timer = 0;

            for (int i = 0; i < projectilesPerBurst; i++)
            {
                GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
                projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;

                yield return new WaitForSeconds(timeBetweenBullets);
            }

        }
    }

    // Fires Bullet //
    private void RegularFire()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenBullets)
        {
            GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);

            projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;

            timer = 0;
        }
    }

    private void ThrowGrenade()
    {
        timer += Time.deltaTime;
        gTimer += Time.deltaTime;
        //Debug.Log("Timer " + timer + " - timeBetweenGrenade" + timeBetweenGrenade);
        if (gTimer >= timeBetweenGrenade)
        {
            throwing = true;
            hasThrown = false;
            StartCoroutine("Throw", waitTime);
            //Debug.Log("GRENADE");
        }
        if (timer >= timeBetweenBullets && throwing == false)
        {
            GameObject projectileFired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);

            projectileFired.GetComponent<ProjectileBase>().projectileDamage = damage;

            timer = 0;
        }
        
    }

    private void ThrowGrenadeCheck()
    {
        if (hasThrown == false)
        {
            GameObject grenade = Instantiate(grenadePrefab, firePoint.transform.position, firePoint.transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            hasThrown = true;
        }

    }

    IEnumerator Throw()
    {
        //Stop firing 
        //throwing = true;
        yield return new WaitForSeconds(waitTime);

        ThrowGrenadeCheck();
        gTimer = 0;

        yield return new WaitForSeconds(waitTime);
        throwing = false;
    }
}
