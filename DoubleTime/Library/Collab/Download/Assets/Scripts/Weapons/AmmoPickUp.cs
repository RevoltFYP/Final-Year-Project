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

            if (weapBase.currentAmmo < weapBase.ammo)
            {
                weapBase.currentAmmo += amount;

                // after adding curr ammo exceeds max ammo
                if (weapBase.currentAmmo > weapBase.ammo)
                {
                    weapBase.currentAmmo = weapBase.ammo;
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
