using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapScript : MonoBehaviour {

    [Header("Number in Weapon Inventory Array")]
    public int amount;

    public float destroyIn;

    private void OnEnable()
    {
        Invoke("Destroy", destroyIn);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            // Adds amount to player's scrap
            ScrapManager scrapManager = other.gameObject.GetComponent<ScrapManager>();
            scrapManager.AddScrap(amount);
            Destroy();
        }
    }

    private void Destroy()
    {
        //Debug.Log("Ammo Destroyed");
        gameObject.SetActive(false);
    }
}
