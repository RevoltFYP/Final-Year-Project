using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBoomarang : MonoBehaviour {

    public GameObject Boomarang;
    public static bool haveBoomarang = true;

	// Update is called once per frame
	void Update ()
    {
        ShootBoomarang_();
    }

    void ShootBoomarang_()
    {
        if (Input.GetButtonDown("Fire2") & haveBoomarang == true)
        {
            Instantiate(Boomarang, transform.position, transform.rotation);
            haveBoomarang = false;
        }
    }
}
