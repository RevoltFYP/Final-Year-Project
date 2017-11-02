using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeWeapon : MonoBehaviour {

    public KeyCode interactKey;

    public GameObject upgradeUI;
    public GameObject upgrade;
    public GameObject pickWeapon;
    public GameObject Weap_upgrade;
    //public GameObject SG_upgrade;

    [Header("Images")]
    public Image weapImg;
    public Sprite upgradeMG;
    public Sprite upgradeSG;

    [Header("Stats UI")]
    public Slider damageSlider;
    public Slider ammoSlider;
    public Slider spreadSlider;
    private int damageIncrease;
    private int ammoIncrease;
    private int spreadDecrease;
    public Text currencyText;

    [Header("Machine Gun")]
    public GameObject machineGun;
    public int MG_damageMax;
    public int MG_ammoMax;
    public int MG_spreadMax;
    public int MG_damageIncrease;
    public int MG_ammoIncrease;
    public int MG_spreadDecrease;

    [Header("Shot Gun")]
    public GameObject shotGun;
    public int SG_damageMax;
    public int SG_ammoMax;
    public int SG_spreadMax;
    public int SG_damageIncrease;
    public int SG_ammoIncrease;
    public int SG_spreadDecrease;

    private GameObject weaponToUpgrade;
    private WeaponBase weapBase;
    private bool display;
    private ScrapManager scrapManager;

    [Header("Disabled Scripts")]
    public PlayerCamera cameraScript;
    private PlayerMovement playerMove;
    private SlowTimeScript slowScript;

    [Header("Data Manager")]
    public DataManager dataManager;
    private ShotGunScript shotGunScript;
    private WeaponBase machineGunScript;

    private void Awake()
    {
        weapImg.preserveAspect = true;

        if (dataManager != null)
        {
            shotGunScript = shotGun.GetComponent<ShotGunScript>();
            machineGunScript = machineGun.GetComponent<WeaponBase>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shop")
        {
            scrapManager = GetComponent<ScrapManager>();
            playerMove = GetComponent<PlayerMovement>();
            slowScript = GetComponent<SlowTimeScript>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Shop")
        {
            //Debug.Log("true");

            if (Input.GetKeyDown(interactKey))
            {
                display = !display;
                UpgradeShop(display);
            }
            //Debug.Log(Time.timeScale);
        }
    }

    public void UpgradeShop(bool show)
    {
        upgradeUI.gameObject.SetActive(show);
        upgrade.gameObject.SetActive(!show);
        pickWeapon.gameObject.SetActive(show);
        Weap_upgrade.SetActive(false);
        //SG_upgrade.SetActive(false);

        // Disable unwanted scripts
        playerMove.enabled = !show;
        slowScript.enabled = !show;
        cameraScript.enabled = !show;
        machineGun.GetComponent<WeaponBase>().enabled = !show;
        shotGun.GetComponent<ShotGunScript>().enabled = !show;

        // Pause game
        //Time.timeScale = show ? 0 : 1;
    }

    public void UpgradeDamage(int cost)
    {
        if(weapBase.damagePerShot < damageSlider.maxValue && scrapManager.scrap >= cost)
        {
            // Reduce scrap by cost 
            scrapManager.RemoveScrap(cost);

            weapBase.damagePerShot += damageIncrease;
            damageSlider.value = weapBase.damagePerShot;

            // Save data to be loaded
            SaveData();
        }

        Debug.Log(weaponToUpgrade.name + ": " + weapBase.damagePerShot);
    }

    public void UpgradeAmmo(int cost)
    {
        if(weapBase.ammo < ammoSlider.maxValue && scrapManager.scrap >= cost)
        {
            // Reduce scrap by cost 
            scrapManager.RemoveScrap(cost);

            weapBase.ammo += ammoIncrease;
            ammoSlider.value = weapBase.ammo;

            // Set current ammo to max
            weapBase.currentAmmo = weapBase.ammo;

            // Save data to be loaded
            SaveData();
        }

        Debug.Log(weaponToUpgrade.name + ": " + weapBase.ammo);
    }

    public void UpgradeBulletSpread(int cost)
    {
        if(scrapManager.scrap >= cost)
        {
            if (weapBase.bulletSpread > 0)
            {
                // Reduce scrap by cost 
                scrapManager.RemoveScrap(cost);

                weapBase.bulletSpread -= spreadDecrease;
                spreadSlider.value = weapBase.bulletSpread;

                // Save data to be loaded
                SaveData();
            }
            else
            {
                weapBase.bulletSpread = 0;
            }
        }
        Debug.Log(weaponToUpgrade.name + ": " + weapBase.bulletSpread);
    }

    public void PickWeapon(GameObject weapon)
    {
        weaponToUpgrade = weapon;

        // Set max values for Gun
        if (weaponToUpgrade == machineGun)
        {
            weapBase = weaponToUpgrade.GetComponent<WeaponBase>();
            weapImg.sprite = upgradeMG;

            damageSlider.maxValue = MG_damageMax;
            ammoSlider.maxValue = MG_ammoMax;
            spreadSlider.maxValue = MG_spreadMax;

            damageIncrease = MG_damageIncrease;
            ammoIncrease = MG_ammoIncrease;
            spreadDecrease = MG_spreadDecrease;
        }
        else
        {
            weapBase = weaponToUpgrade.GetComponent<ShotGunScript>();
            weapImg.sprite = upgradeSG;

            damageSlider.maxValue = SG_damageMax;
            ammoSlider.maxValue = SG_ammoMax;
            spreadSlider.maxValue = SG_spreadMax;

            damageIncrease = SG_damageIncrease;
            ammoIncrease = SG_ammoIncrease;
            spreadDecrease = SG_spreadDecrease;
        }

        // Show selected weapon stats
        damageSlider.value = weapBase.damagePerShot;
        ammoSlider.value = weapBase.ammo;
        spreadSlider.value = weapBase.bulletSpread;
        currencyText.text = "Scrap: " + scrapManager.scrap.ToString();

        //Debug.Log(weaponToUpgrade.name + " Damage : " + weapBase.damagePerShot);
        //Debug.Log(weaponToUpgrade.name + " Spread : " + weapBase.bulletSpread);
        //Debug.Log(weaponToUpgrade.name + " Ammo : " + weapBase.ammo);
    }

    private void SaveData()
    {
        if(dataManager != null)
        {
            // Save data to be loaded
            dataManager.SaveWeaponStats(shotGunScript, machineGunScript);
        }
        else
        {
            Debug.LogWarning("No Data Manager");
        }
    }
}
