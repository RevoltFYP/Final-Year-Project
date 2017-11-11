using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int recoverAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealth playerHpScript = other.gameObject.GetComponent<PlayerHealth>();

            if(playerHpScript.currentHealth <= playerHpScript.startingHealth)
            {
                playerHpScript.currentHealth += recoverAmount;

                if(playerHpScript.currentHealth >= playerHpScript.startingHealth)
                {
                    playerHpScript.currentHealth = playerHpScript.startingHealth;
                }
            }
        }
    }
}
