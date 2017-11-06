using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBoomarang : MonoBehaviour {

    public GameObject Boomarang;
    public bool haveBoomarang { get; set; }

    private void Awake()
    {
        haveBoomarang = true;
    }

    // Update is called once per frame
    void Update ()
    {
        ShootBoomarang_();
    }

    void ShootBoomarang_()
    {
        if (Input.GetButtonDown("Fire2") && haveBoomarang && !GetComponent<WeaponInventory>().currentWeapon.GetComponent<WeaponBase>().firing)
        {
            Debug.Log("True");
            Instantiate(Boomarang, transform.position + transform.forward, transform.rotation);
            haveBoomarang = false;
        }
    }
}
