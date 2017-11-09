using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour {
    [Header("Number in Weapon Inventory Array")]
    public int ammoType;
    public int amount;

    public float destroyIn;

    private void OnEnable()
    {
        Invoke("Destroy", destroyIn);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.tag == "Player")
        {
            // Works for anything that inherits from weaponBase
            WeaponInventory weapInven = other.gameObject.GetComponent<WeaponInventory>();
            WeaponBase weapBase = weapInven.weaponInventory[ammoType].GetComponent<WeaponBase>();

            if(weapBase.currentAmmo < weapBase.magazineSize || weapBase.totalAmmo < weapBase.maxAmmo)
            {
                for (int i = 0; i < amount; i++)
                {
                    if (weapBase.currentAmmo < weapBase.magazineSize)
                    {
                        weapBase.currentAmmo += 1;
                    }
                    else
                    {
                        if (weapBase.totalAmmo < weapBase.maxAmmo)
                        {
                            weapBase.totalAmmo += 1;
                        }
                    }
                }
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        //Debug.Log("Ammo Destroyed");
        gameObject.SetActive(false);
    }
}
