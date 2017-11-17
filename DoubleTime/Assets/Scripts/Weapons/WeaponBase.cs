﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Light))]
// Ensure that the object's parent is Player //
public class WeaponBase : MonoBehaviour {

    private WeaponInventory weapInvenReference;

    [Header("Gun Stats")]
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public Transform firePoint;

    [Header("Bullet Stats")]
    public float bulletForce = 30.0f;
    public float bulletSpread = 15.0f;
    public GameObject bullet;

    [Header("Ammo Stats")]
    public bool infiniteAmmo;
    public Image reloadImage;
    public int magazineSize;
    public int maxAmmo = 10;
    public int ammoPerShot = 1;
    public float reloadTime = 2;
    private bool isReloading = false;
    private Image reloadFill;
    private float reloadTimer = 0;

    [Header("Object Polling")]
    public int pooledAmount = 30;
    public List<GameObject> projectiles { get; set; }

    private GameObject player;
    public int currentAmmo { get; set; }
    public int totalAmmo { get; set; }
    private float timer { get; set; }
    public int shootableMask { get; set; }
    private Light gunLight { get; set; }
    private float effectsDisplayTime = 0.2f;
    public bool firing { get; set; }
    private ShootBoomarang shootBoomerang;

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

        shootBoomerang = transform.parent.GetComponent<ShootBoomarang>();

        //player = transform.parent.gameObject;
        shootableMask = LayerMask.GetMask("Shootable");
        gunLight = GetComponent<Light>();

        totalAmmo = maxAmmo;
        currentAmmo = magazineSize;

        reloadImage.gameObject.SetActive(false);
        reloadFill = reloadImage.transform.GetChild(0).GetComponent<Image>();
        reloadTimer = reloadTime;
    }

    private void OnEnable()
    {
        reloadImage.fillAmount = 1;

        isReloading = false;
    }

    void Update()
    {
        timer += Time.deltaTime * 1/Time.timeScale;

        if (isReloading)
        {
            ReloadUI();
            return;
        }

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetKey(weapInvenReference.fireKey))
        {
            if(timer >= timeBetweenBullets && currentAmmo > 0 && weapInvenReference.canFire && shootBoomerang.haveBoomarang)
            {
                firing = true;
                Shoot();
            }
        }
        else
        {
            firing = false;
        }

        if (Input.GetKeyDown(weapInvenReference.reloadKey))
        {
            StartCoroutine(Reload());
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
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
                projectiles[i].transform.position = firePoint.position;
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
    public void ResetTimer()
    {
        timer = 0f;
    }

    public IEnumerator Reload()
    {
        if (currentAmmo < magazineSize)
        {
            isReloading = true;
            //Debug.Log("Reloading");

            // Find missing amount of ammo
            int missingAmmo = magazineSize - currentAmmo;

            yield return new WaitForSeconds(reloadTime);

            if (!infiniteAmmo)
            {
                // total ammo has enough ammo for missing
                if (totalAmmo >= missingAmmo)
                {
                    // Add missing ammo to current ammo
                    currentAmmo += missingAmmo;

                    // Subtract missing ammo from total ammo;
                    totalAmmo -= missingAmmo;
                }
                // total ammo does not have enough
                else
                {
                    // add remainder of ammo to current ammo
                    currentAmmo += totalAmmo;
                    totalAmmo = 0;
                }
            }
            else
            {
                currentAmmo += missingAmmo;
            }

            isReloading = false;
        }
    }

    // Reload UI
    private void ReloadUI()
    {
        if (reloadImage != null && reloadFill != null)
        {
            // Show UI
            reloadImage.gameObject.SetActive(true);

            if (reloadTimer <= reloadTime && reloadTimer != 0)
            {
                // Set Fill amount
                reloadTimer -= Time.deltaTime;
                reloadFill.fillAmount = reloadTimer / reloadTime;

                // Fill amount finishes
                if (reloadFill.fillAmount == 0)
                {
                    // reset all objects
                    reloadFill.fillAmount = 1.0f;
                    reloadImage.gameObject.SetActive(false);
                    reloadTimer = reloadTime;
                }
            }
        }
    }

}
