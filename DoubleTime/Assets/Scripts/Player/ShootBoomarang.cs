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
    private bool cdBoom;

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

        if (cdBoom)
        {
            //boomarang CD
            boomarangCDTimer -= Time.deltaTime * 1 / Time.timeScale;

            BoomerangUI();

            if (boomarangCDTimer <= 0)
            {
                GetComponent<ShootBoomarang>().haveBoomarang = true;
                boomarangCDTimer = boomarangCD;
            }
        }
    }

    void ShootBoomarang_()
    {
        if (Input.GetButtonDown("Fire2") && haveBoomarang && !GetComponent<WeaponInventory>().currentWeapon.GetComponent<WeaponBase>().firing && !cdBoom)
        {
            Instantiate(Boomarang, transform.position + transform.forward, transform.rotation);
            haveBoomarang = false;

            // UI
            boomerangImgChild.fillAmount = 0;
            cdBoom = true;
        }
    }

    private void BoomerangUI()
    {
        if (boomerangImgChild != null)
        {
            if (boomerangImageTimer <= boomarangCD && boomarangCDTimer != 0)
            {
                // Set Fill amount
                boomerangImageTimer += Time.deltaTime * 1 / Time.timeScale;
                boomerangImgChild.fillAmount = boomerangImageTimer / boomarangCD;

                if (boomerangImgChild.fillAmount == 1)
                {
                    cdBoom = false;
                    boomerangImageTimer = 0;
                }
            }
        }
    }
}
