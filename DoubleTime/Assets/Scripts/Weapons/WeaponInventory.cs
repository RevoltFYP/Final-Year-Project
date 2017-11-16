using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour {

    public KeyCode fireKey;
    public KeyCode reloadKey;
    public List<GameObject> weaponInventory = new List<GameObject>(); // all weapons in player 

    public GameObject startWeapon; // starting weapon of player
    public GameObject currentWeapon { get; set; }

    private int weaponsLayerNumber = 10;
    private int enemyLayerNumber = 9;

    private int bulletIgnoreCollisionLayer = 14;

    public List<KeyCode> keyCodes = new List<KeyCode>();

    /*[Header("Weapon Data")]
    public DataManager dataManager;
    public bool resetWeapons;
    private ShotGunScript shotGunScript;
    private WeaponBase machineGunScript;*/

    [Header("Ammo UI")]
    public Text currentAmmoText;
    public Text maxAmmoText;
    public Slider ammoSlider;

    [Header("Weapon UI")]
    public Image currentWeaponImage;
    public List<Sprite> weaponImages = new List<Sprite>();

    [Header("Models")]
    public GameObject protagonistObj;
    public List<GameObject> weaponMeshes = new List<GameObject>();
    private Animator animator;

    public bool canFire { get; set; }

    private int selectedWeapon;
    // Use this for initialization
    void Awake () {
        canFire = true;

        animator = protagonistObj.GetComponent<Animator>();

        // Ignores collision between weapons and enemies
        Physics.IgnoreLayerCollision(weaponsLayerNumber, enemyLayerNumber);

        // Ignores collision between bullets of the same layer
        Physics.IgnoreLayerCollision(bulletIgnoreCollisionLayer, bulletIgnoreCollisionLayer, true);

        currentWeapon = startWeapon;

        // UI initialization
        if (!this.enabled)
        {
            currentAmmoText.gameObject.SetActive(false);
            maxAmmoText.gameObject.SetActive(false);
            currentWeaponImage.gameObject.SetActive(false);
            ammoSlider.gameObject.SetActive(false);
        }

        // Check whether to load data or reset
        /*if(dataManager != null)
        {
            if (resetWeapons)
            {
                dataManager.ResetWeapons();
            }
            else
            {
                dataManager.LoadWeaponStats(weaponInventory[2].GetComponent<ShotGunScript>(), weaponInventory[1].GetComponent<WeaponBase>());
            }
        }*/
    }

    private void OnEnable()
    {
        currentWeapon.SetActive(true);
        currentWeaponImage.gameObject.SetActive(true);
        currentAmmoText.gameObject.SetActive(true);
        maxAmmoText.gameObject.SetActive(true);
        ammoSlider.gameObject.SetActive(true);

        for (int i = 0; i < weaponInventory.Count; i++)
        {
            if (currentWeapon == weaponInventory[i])
            {
                WeaponSwitch(i);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(currentWeapon != null)
        {
            //Debug.Log("Current Weapon: " + currentWeapon);
            //Debug.Log("Weapon Inventory [0]: " + weaponInventory[0]);
            AmmoUI();

            for (int i = 0; i < keyCodes.Count; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    // Set Current Weapon to false
                    currentWeapon.SetActive(false);

                    // Change current weapon to weapon swapped to
                    currentWeapon = weaponInventory[i];

                    // Display new current weapon
                    currentWeapon.SetActive(true);

                    // Plays weapon switch animation
                    WeaponSwitch(i);
                }
            }
        }
    }

    private void CurrentWeaponImage(int weapNumber)
    {
    }

    private void AmmoUI()
    {
        ammoSlider.gameObject.SetActive(true);

        if (currentWeapon.GetComponent<ShotGunScript>())
        {
            currentAmmoText.text = currentWeapon.GetComponent<ShotGunScript>().currentAmmo.ToString();
            maxAmmoText.text = currentWeapon.GetComponent<ShotGunScript>().totalAmmo.ToString();

            ammoSlider.value = currentWeapon.GetComponent<ShotGunScript>().currentAmmo;
            ammoSlider.maxValue = currentWeapon.GetComponent<ShotGunScript>().magazineSize;
        }
        else
        {
            currentAmmoText.text = currentWeapon.GetComponent<WeaponBase>().currentAmmo.ToString();
            maxAmmoText.text = currentWeapon.GetComponent<WeaponBase>().totalAmmo.ToString();

            ammoSlider.value = currentWeapon.GetComponent<WeaponBase>().currentAmmo;
            ammoSlider.maxValue = currentWeapon.GetComponent<WeaponBase>().magazineSize;
        }
    }

    private void WeaponSwitch(int weaponNumber)
    {
        if(currentWeapon != null)
        {
            foreach (GameObject mesh in weaponMeshes)
            {
                mesh.SetActive(false);
            }

            animator.SetBool("WeaponEquipped", true);
            weaponMeshes[weaponNumber].SetActive(true);
            currentWeaponImage.sprite = weaponImages[weaponNumber];
        }
    }
}
