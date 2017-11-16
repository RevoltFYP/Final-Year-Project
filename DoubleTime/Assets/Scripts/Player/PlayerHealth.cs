using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 100;
    public int currentHealth;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [Header("HP Bar")]
    public GameObject healthFrame;
    public RectTransform healthTransform;
    public RectTransform handleTransform;
    public Sprite damageSprite;
    public Sprite damageFrame;
    public Text healthText;
    private float cachedY;
    private float cachedZ;
    private float minX;
    private float maxX;
    private Sprite originalSprite;
    private Sprite originalFrame;

    [Header("Death")]
    public GameObject ragDoll;
    public GameObject charac;

    [Header("Med Kit")]
    public KeyCode medKitKey;
    public int recoverAmount;
    public int maxMedKit;
    public List<GameObject> medKitImages = new List<GameObject>();

    public int currentMedKit { get; set; }
    private GameObject medKitGroup;

    private bool isDead;
    private bool isDamaged;

    void Awake ()
    {
        originalSprite = healthTransform.GetComponent<Image>().sprite;
        originalFrame = healthFrame.GetComponent<Image>().sprite;

        cachedY = healthTransform.localPosition.y;
        cachedZ = healthTransform.localPosition.z;
        maxX = healthTransform.localPosition.x;
        minX = healthTransform.localPosition.x - healthTransform.rect.width;

        currentHealth = startingHealth;
    }

    void Update ()
    {
        if (Input.GetKeyDown(medKitKey))
        {
            UseMedKit();
            UpdateHealthBar();
        }

        if(isDamaged)
        {
            // Flashes when damaged 
            damageImage.color = flashColour;
            healthTransform.GetComponent<Image>().sprite = damageSprite;
            healthFrame.GetComponent<Image>().sprite = damageFrame;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.unscaledDeltaTime);
            healthTransform.GetComponent<Image>().sprite = originalSprite;
            healthFrame.GetComponent<Image>().sprite = originalFrame;
        }

        isDamaged = false;
    }

    private void UpdateHealthBar()
    {
        healthText.text = currentHealth.ToString();
        float currentXValue = MapValues(currentHealth, 0, startingHealth, minX, maxX);
        healthTransform.localPosition = new Vector3(currentXValue, cachedY, cachedZ);
        handleTransform.localPosition = new Vector3(currentXValue, cachedY, cachedZ);

        if (isDead)
        {
            healthTransform.gameObject.SetActive(false);
            handleTransform.gameObject.SetActive(false);
        }
    }

    public void TakeDamage (int amount)
    {
        isDamaged = true;

        // Reduces current health 
        currentHealth -= amount;

        // When health reaches 0 player is dead 
        if(currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            Death();
        }

        UpdateHealthBar();
    }

    void Death ()
    {
        UpdateHealthBar();

        isDead = true;

        // Disables character movement 
        GetComponent<PlayerMovement>().enabled = false;

        // Replace current model with Rag Doll
        if (!ragDoll.activeSelf)
        {
            ragDoll.SetActive(true);
            charac.SetActive(false);
        }

        // Disable Weapons
        WeaponInventory weapInven = GetComponent<WeaponInventory>();
        weapInven.currentWeapon.SetActive(false);
        weapInven.enabled = false;

        // Disable slow time
        GetComponent<SlowTimeScript>().enabled = false;

        // Player Pause disable
        GetComponent<PlayerPause>().enabled = false;

        // Disable player melee
        GetComponent<PlayerMelee>().enabled = false;

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void UseMedKit()
    {
        if(currentMedKit > 0)
        {
            if (currentHealth < startingHealth)
            {
                currentHealth += recoverAmount;

                if (currentHealth > startingHealth)
                {
                    currentHealth = startingHealth;
                }

                currentMedKit -= 1;

                UpdateMedKitUI();
            }
        }
    }

    public void UpdateMedKitUI()
    {
        foreach(GameObject image in medKitImages)
        {
            image.SetActive(false);
        }

        if(currentMedKit > 0)
        {
            for(int i = 0; i < currentMedKit; i++)
            {
                medKitImages[i].SetActive(true);
            }
        }
    }

    public float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
