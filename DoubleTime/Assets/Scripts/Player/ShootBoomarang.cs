using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootBoomarang : MonoBehaviour {

    public GameObject Boomarang;
    public bool haveBoomarang { get; set; }

    [Header("Boomarang CD")]
    public float boomarangCD = 5f;
    private float boomarangCDTimer;

    [Header("Boomarang UI")]
    public Image boomerangImage;
    private Image boomerangImgChild;
    private float boomerangImageTimer;

    private void Awake()
    {
        haveBoomarang = true;
        boomerangImgChild = boomerangImage.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update ()
    {
        ShootBoomarang_();

        //boomarang CD
        boomarangCDTimer -= Time.deltaTime * 1 / Time.timeScale;
        //Debug.Log(boomarangCDTimer+"-");

        if (boomarangCDTimer <= 0)
        {
            //Debug.Log("True");
            GetComponent<ShootBoomarang>().haveBoomarang = true;
            boomarangCDTimer = boomarangCD;
        }

        if (boomarangCDTimer != 0)
        {
            BoomerangUI();
        }
    }

    void ShootBoomarang_()
    {
        if (Input.GetButtonDown("Fire2") && haveBoomarang && !GetComponent<WeaponInventory>().currentWeapon.GetComponent<WeaponBase>().firing)
        {
            Instantiate(Boomarang, transform.position + transform.forward, transform.rotation);
            haveBoomarang = false;
            boomerangImgChild.fillAmount = 0;
        }
    }

    private void BoomerangUI()
    {
        if (boomerangImage != null && boomerangImgChild != null)
        {
            // Show UI
            boomerangImage.gameObject.SetActive(true);

            if (boomerangImageTimer <= boomarangCD && boomarangCDTimer != 0)
            {
                // Set Fill amount
                boomerangImageTimer += Time.deltaTime;
                boomerangImgChild.fillAmount = boomerangImageTimer / boomarangCD;

                // Fill amount finishes
                if (boomerangImgChild.fillAmount == 1)
                {
                    // reset all objects
                    boomerangImage.gameObject.SetActive(false);
                }
            }
        }
    }
}
