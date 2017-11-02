using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour {

    public KeyCode fireKey;
    public List<GameObject> weaponInventory = new List<GameObject>(); // all weapons in player 

    public GameObject startWeapon; // starting weapon of player
    public GameObject currentWeapon { get; set; }

    private int weaponsLayerNumber = 10;
    private int enemyLayerNumber = 9;

    private int bulletIgnoreCollisionLayer = 14;

    public List<KeyCode> keyCodes = new List<KeyCode>();

    [Header("Ammo Drops")]
    public int pooledAmount = 10;
    public GameObject[] ammoTypes;
    public List<GameObject> ammoDrops { get; set; }

    [Header("Weapon Data")]
    public DataManager dataManager;
    public bool resetWeapons;
    private ShotGunScript shotGunScript;
    private WeaponBase machineGunScript;

    [Header("Boomarang CD")]
    public float boomarangCD = 5f;
    private float boomarangCDTimer;

    [Header("Ammo UI")]
    public Text currentAmmoText;
    public Text maxAmmoText;
    public Slider ammoSlider;

    [Header("Weapon UI")]
    public Image currentWeaponImage;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public List<Image> weaponToolBar = new List<Image>();
    public List<Sprite> selectedWeaponSprite = new List<Sprite>();
    public List<Sprite> unselectedWeaponImage = new List<Sprite>();

    private Animator animator;
    public GameObject protagonistObj;
    public GameObject weaponObj;

    public bool canFire { get; set; }

    private int selectedWeapon;
    // Use this for initialization
    void Awake () {
        canFire = true;

        animator = protagonistObj.GetComponent<Animator>();

        ammoDrops = new List<GameObject>();

        foreach(GameObject ammo in ammoTypes)
        {
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(ammo);
                ammoDrops.Add(obj);
                obj.SetActive(false);
                GameObject.DontDestroyOnLoad(obj);
            }
        }

        // Ignores collision between weapons and enemies
        Physics.IgnoreLayerCollision(weaponsLayerNumber, enemyLayerNumber);

        // Ignores collision between bullets of the same layer
        Physics.IgnoreLayerCollision(bulletIgnoreCollisionLayer, bulletIgnoreCollisionLayer, true);

        currentWeapon = startWeapon;

        // UI initialization
        if (this.enabled)
        {
            UpdateWeaponToolbar();
        }
        else
        {
            currentAmmoText.gameObject.SetActive(false);
            maxAmmoText.gameObject.SetActive(false);
            currentWeaponImage.gameObject.SetActive(false);
            ammoSlider.gameObject.SetActive(false);
        }

        // Check whether to load data or reset
        if (resetWeapons)
        {
            dataManager.ResetWeapons();
        }
        else
        {
            dataManager.LoadWeaponStats(weaponInventory[2].GetComponent<ShotGunScript>(), weaponInventory[1].GetComponent<WeaponBase>());
        }
    }

    private void OnEnable()
    {
        currentWeapon.SetActive(true);
        currentWeaponImage.gameObject.SetActive(true);

        if (currentWeapon != weaponInventory[0])
        {
            currentAmmoText.gameObject.SetActive(true);
            maxAmmoText.gameObject.SetActive(true);
            ammoSlider.gameObject.SetActive(true);
        }

        for (int i = 0; i < weaponInventory.Count; i++)
        {
            if (currentWeapon == weaponInventory[i])
            {
                SelectedWeapon(i);
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

            //boomarang CD
            boomarangCDTimer -= Time.deltaTime * 1 / Time.timeScale;
            //Debug.Log(boomarangCDTimer+"-");

            if (boomarangCDTimer <= 0)
            {
                ShootBoomarang.haveBoomarang = true;
                boomarangCDTimer = boomarangCD;
            }

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

                    // Set new selected sprite
                    weaponToolBar[i].sprite = selectedSprite;

                    // Set UI for selected Weapon
                    SelectedWeapon(i);

                    // Plays weapon switch animation
                    WeaponSwitch();
                }
            }
        }
    }

    // Updates weapon toolbar UI in case any weapons have been added
    public void UpdateWeaponToolbar()
    {
        for (int i = 0; i < keyCodes.Count; i++)
        {
            weaponToolBar[i].gameObject.SetActive(true);
        }
    }

    private void AmmoUI()
    {
        if (currentWeapon != weaponInventory[0])
        {
            ammoSlider.gameObject.SetActive(true);

            if (currentWeapon.GetComponent<ShotGunScript>())
            {
                currentAmmoText.text = currentWeapon.GetComponent<ShotGunScript>().currentAmmo.ToString();
                maxAmmoText.text = "/" + currentWeapon.GetComponent<ShotGunScript>().ammo.ToString();

                ammoSlider.value = currentWeapon.GetComponent<ShotGunScript>().currentAmmo;
                ammoSlider.maxValue = currentWeapon.GetComponent<ShotGunScript>().ammo;
            }
            else
            {
                currentAmmoText.text = currentWeapon.GetComponent<WeaponBase>().currentAmmo.ToString();
                maxAmmoText.text = "/" + currentWeapon.GetComponent<WeaponBase>().ammo.ToString();

                ammoSlider.value = currentWeapon.GetComponent<WeaponBase>().currentAmmo;
                ammoSlider.maxValue = currentWeapon.GetComponent<WeaponBase>().ammo;
            }
        }
        else
        {
            //Debug.Log("Current Weapon is Boomerang");
            currentAmmoText.text = string.Empty;
            maxAmmoText.text = string.Empty;
            ammoSlider.gameObject.SetActive(false);
        }
    }

    private void SelectedWeapon(int number)
    {
        // Reset all tool bar UI
        for(int i = 0; i < weaponToolBar.Count; i++)
        {
            weaponToolBar[i].sprite = unselectedSprite;
            weaponToolBar[i].transform.GetChild(0).GetComponent<Image>().sprite = unselectedWeaponImage[i];
        }

        weaponToolBar[number].sprite = selectedSprite;
        weaponToolBar[number].transform.GetChild(0).GetComponent<Image>().sprite = selectedWeaponSprite[number];

        // Set sprite of current weapon
        currentWeaponImage.sprite = selectedWeaponSprite[number];
    }

    private void WeaponSwitch()
    {
        if (currentWeapon != weaponInventory[0])
        {
            animator.SetBool("WeaponEquipped", true);
            weaponObj.SetActive(true);
        }
        else
        {
            animator.SetBool("WeaponEquipped", false);
            weaponObj.SetActive(false);
        }
    }
}
