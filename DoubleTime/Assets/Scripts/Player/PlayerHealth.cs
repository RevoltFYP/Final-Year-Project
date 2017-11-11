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

    [Header("Med Kit")]
    public KeyCode medKitKey;
    public int recoverAmount;
    public int maxMedKit;
    public Image kitImage;
    public Sprite emptySprite;
    public Sprite filledSprite;

    public int currentMedKit { get; set; }
    private GameObject medKitGroup;
    private List<Image> medKitImages = new List<Image>();

    private bool isDead;
    private bool isDamaged;

    void Awake ()
    {
        currentHealth = startingHealth;
        originalColour = hpFill.color;

        medKitGroup = GameObject.Find("MedKitImages");

        for (int i = 0; i < maxMedKit; i++)
        {
            Image img = (Image)Instantiate(kitImage);
            img.transform.SetParent(medKitGroup.transform);
            medKitImages.Add(img);

            img.gameObject.SetActive(true);
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(medKitKey))
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

                // Update UI
                healthSlider.value = currentHealth;
                UpdateMedKitUI();
            }
        }
    }

    public void UpdateMedKitUI()
    {
        foreach(Image image in medKitImages)
        {
            image.sprite = emptySprite;
        }

        if(currentMedKit > 0)
        {
            for(int i = 0; i < currentMedKit; i++)
            {
                medKitImages[i].sprite = filledSprite;
            }
        }
    }
}
