using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public float destroyIn;

    private void OnEnable()
    {
        Invoke("Destroy", destroyIn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealth playerHpScript = other.gameObject.GetComponent<PlayerHealth>();

            if(playerHpScript.currentMedKit < playerHpScript.maxMedKit)
            {
                playerHpScript.currentMedKit += 1;
                playerHpScript.UpdateMedKitUI();

                Destroy();
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
