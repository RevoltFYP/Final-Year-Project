using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]

// Ensure that the object's parent is Player //
public class WeaponBase : MonoBehaviour {

    private WeaponInventory weapInvenReference;

    [Header("Gun Stats")]
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;

    [Header("Bullet Stats")]
    public float bulletForce = 30.0f;
    public float bulletSpread = 15.0f;
    public GameObject bullet;

    [Header("Ammo Stats")]
    public int ammo = 10;
    public int ammoPerShot = 1;

    [Header("Object Polling")]
    public int pooledAmount = 30;
    public List<GameObject> projectiles { get; set; }

    private GameObject player;
    public int currentAmmo { get; set; }
    private float timer { get; set; }
    public int shootableMask { get; set; }
    private Light gunLight { get; set; }
    private float effectsDisplayTime = 0.2f;

    void Awake()
    {
        weapInvenReference = transform.parent.GetComponent<WeaponInventory>();
        projectiles = new List<GameObject>();

        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            projectiles.Add(obj);
            obj.SetActive(false);
            GameObject.DontDestroyOnLoad(obj);
        }

        //player = transform.parent.gameObject;
        shootableMask = LayerMask.GetMask("Shootable");
        gunLight = GetComponent<Light>();
        currentAmmo = ammo;
    }

    void Update()
    {
        timer += Time.deltaTime * 1/Time.timeScale;

        if(Input.GetKey(weapInvenReference.fireKey) && timer >= timeBetweenBullets && currentAmmo > 0 && weapInvenReference.canFire)
        {
            Shoot();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        //Debug.Log(this.name + ": " + damagePerShot);
    }

    // Shoots bullets with Spread //
    protected virtual void Shoot()
    {
        ResetTimer();

        currentAmmo -= ammoPerShot;

        EnableEffects();

        // Bullet Spread //
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward); // converts transform forward into Quaternion
        Quaternion randomRotation = Quaternion.Euler(new Vector3(0.0f, Random.rotation.y, 0.0f)); // creates a random rotation value for Y axis
        fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(-bulletSpread, bulletSpread)); // rotates bullet from original to random rotation using bullet spread as range

        //GameObject bulletFired = Instantiate(bullet, transform.position, fireRotation);

        // Spawning Bullets through the use of object pooling
        for(int i =0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                projectiles[i].transform.position = transform.position;
                projectiles[i].transform.rotation = fireRotation;
                projectiles[i].SetActive(true);

                ProjectileBase playerBulletScript = projectiles[i].GetComponent<ProjectileBase>();

                playerBulletScript.projectileDamage = damagePerShot;
                break;
            }
        }
    }
    
    // Disables GunLight and GunLine //
    public void DisableEffects ()
    {
        gunLight.enabled = false;
    }

    // Disables GunLight and GunLine //
    protected void EnableEffects()
    {
        gunLight.enabled = true;
    }

    // Resets Shoot Timer to 0 //
    protected void ResetTimer()
    {
        timer = 0f;
    }
}
