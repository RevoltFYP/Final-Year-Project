using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetEnemyZone : EnemyZone {

    public List<GameObject> activeAssets = new List<GameObject>();
    public List<GameObject> deactiveAssets = new List<GameObject>();

    public bool addWeapon;

    private float destroyTime;

    protected override void DestroyAreaZone()
    {
        enemies.Clear();
        ActivateAssets();
        DeactivateAssets();

        if (addWeapon)
        {
            ActivateWeapon();
            Destroy(this);
            Destroy(gameObject, destroyTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ActivateAssets()
    {
        if (activeAssets.Count > 0)
        {
            for (int i = 0; i < activeAssets.Count; i++)
            {
                if (!activeAssets[i].activeSelf)
                {
                    activeAssets[i].SetActive(true);
                }
            }
        }
    }

    private void DeactivateAssets()
    {
        if (deactiveAssets.Count > 0)
        {
            for (int i = 0; i < deactiveAssets.Count; i++)
            {
                if (deactiveAssets[i].activeSelf)
                {
                    deactiveAssets[i].SetActive(false);
                }
            }
        }
    }

    private void ActivateWeapon()
    {
        if(GetComponent<WeaponPickup>() != null)
        {
            WeaponPickup weapPick = GetComponent<WeaponPickup>();

            weapPick.WeaponAdd();

            destroyTime = weapPick.displayTime;
        }
    }
}


