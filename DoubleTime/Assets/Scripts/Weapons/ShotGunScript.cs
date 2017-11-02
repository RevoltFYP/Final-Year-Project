using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunScript : WeaponBase {

    public int shellsFired = 8;
    private float angleChange = 0;

    // Fires shellsFired in a Cone Spread //
    protected override void Shoot()
    {
        ResetTimer();

        EnableEffects();

        currentAmmo -= ammoPerShot;

        float distanceBetweenBullets = ((bulletSpread - (-bulletSpread)) / shellsFired); // calculates the distance between each bullet in spread

        // Instantiate shots equal to Shells Fired //
        for (int i = 0; i < shellsFired; i++)
        {
            // Spread Shot //
            Quaternion fireRotation = Quaternion.LookRotation(transform.forward); // converts transform forward into Quaternion
            Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, (-bulletSpread + angleChange), 0)); // set rotation of each bullet

            for (int l = 0; l < projectiles.Count; l++)
            {
                if (!projectiles[l].activeInHierarchy)
                {
                    projectiles[l].transform.position = transform.position;
                    projectiles[l].transform.rotation = fireRotation * bulletRotation;
                    projectiles[l].SetActive(true);

                    ProjectileBase playerBulletScript = projectiles[l].GetComponent<ProjectileBase>();

                    playerBulletScript.projectileDamage = damagePerShot;
                    break;
                }
            }

            angleChange += distanceBetweenBullets;
        }

        // Resets Angle //
        angleChange = 0;
    }
}
