using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [Header("HP Bar")]
    public Color damageColour;
    public Image hpFill;
    private Color originalColour;

    public GameObject ragDoll;
    public GameObject charac;

    [Header("Med Kits")]
    public KeyCode medKey;
    public int recoverAmount;
    public int maxMedKit;
    public Image image;
    public Sprite emptySprite;
    public Sprite filledSprite;

    private GameObject medKitGroup;
    private List<Image> medKitImage = new List<Image>();
    public int currMedKit { get; set; }

    private bool isDead;
    private bool isDamaged;

    void Awake ()
    {
        currentHealth = startingHealth;
        originalColour = hpFill.color;

        currMedKit = 0;

        medKitGroup = GameObject.Find("MedKitImages");

        for (int i = 0; i < maxMedKit; i++)
        {
            Image img = (Image)Instantiate(image);
            img.transform.SetParent(medKitGroup.transform);

            medKitImage.Add(img);
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(medKey))
        {
            UseMedKit();
        }

        if(isDamaged)
        {
            // Flashes when damaged 
            damageImage.color = flashColour;
            hpFill.color = damageColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.unscaledDeltaTime);
            hpFill.color = Color.Lerp(hpFill.color, originalColour, flashSpeed * Time.unscaledDeltaTime);
        }

        isDamaged = false;
    }

    public void TakeDamage (int amount)
    {
        isDamaged = true;

        // Reduces current health 
        currentHealth -= amount;

        // Set slider to same as current health (UI)
        healthSlider.value = currentHealth;

        // When health reaches 0 player is dead 
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death ()
    {
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

    // Use Med Kit to recover HP
    public void UseMedKit()
    {
        // Player has any med kit
        if(currMedKit > 0)
        {
            // Player not at full health
            if (currentHealth < startingHealth)
            {
                // Add health
                currentHealth += recoverAmount;

                if (currentHealth >= startingHealth)
                {
                    currentHealth = startingHealth;
                }

                // reduce med kit
                currMedKit -= 1;

                // Update UI
                healthSlider.value = currentHealth;
                UpdateMedKitUI();
            }
        }
    }

    public void UpdateMedKitUI()
    {
        foreach(Image image in medKitImage)
        {
            image.GetComponent<Image>().sprite = emptySprite;
        }

        if(currMedKit > 0)
        {
            for (int i = 0; i < currMedKit; i++)
            {
                medKitImage[i].GetComponent<Image>().sprite = filledSprite;
            }
        }
    }
}
