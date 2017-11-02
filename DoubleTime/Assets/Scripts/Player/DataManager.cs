using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveWeaponStats(ShotGunScript shotGun, WeaponBase machineGun)
    {
        // Save Shot Gun Data
        PlayerPrefs.SetInt("Shotgun Damage", shotGun.damagePerShot);
        PlayerPrefs.SetInt("Shotgun Ammo", shotGun.ammo);
        PlayerPrefs.SetFloat("Shotgun Spread", shotGun.bulletSpread);

        // Save Machine Gun Data
        PlayerPrefs.SetInt("Machinegun Damage", machineGun.damagePerShot);
        PlayerPrefs.SetInt("Machinegun Ammo", machineGun.ammo);
        PlayerPrefs.SetFloat("Machinegun Spread", machineGun.bulletSpread);
    }

    public void LoadWeaponStats(ShotGunScript shotGun, WeaponBase machineGun)
    {
        // Save Shot Gun Data
        if(PlayerPrefs.HasKey("Shotgun Damage"))
        shotGun.damagePerShot = PlayerPrefs.GetInt("Shotgun Damage");

        if(PlayerPrefs.HasKey("Shotgun Ammo"))
        shotGun.ammo = PlayerPrefs.GetInt("Shotgun Ammo");

        if(PlayerPrefs.HasKey("Shotgun Spread"))
        shotGun.bulletSpread = PlayerPrefs.GetFloat("Shotgun Spread");

        // Save Machine Gun Data
        if (PlayerPrefs.HasKey("Machinegun Damage"))
        machineGun.damagePerShot = PlayerPrefs.GetInt("Machinegun Damage");

        if (PlayerPrefs.HasKey("Machinegun Ammo"))
        machineGun.ammo = PlayerPrefs.GetInt("Machinegun Ammo");

        if (PlayerPrefs.HasKey("Machinegun Spread"))
        machineGun.bulletSpread = PlayerPrefs.GetInt("Machinegun Spread");
    }

    public void ResetWeapons()
    {
        // Shot gun
        PlayerPrefs.DeleteKey("Shotgun Damage");
        PlayerPrefs.DeleteKey("Shotgun Ammo");
        PlayerPrefs.DeleteKey("Shotgun Spread");

        PlayerPrefs.DeleteKey("Machinegun Damage");
        PlayerPrefs.DeleteKey("Machinegun Ammo");
        PlayerPrefs.DeleteKey("Machinegun Spread");
    }
}
