using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour {

    public GameObject parentObject;
    public EnemyHealth enemyHealth;

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    private PlayerHealth playerHealth;
    private bool playerInRange;
    private float timer;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("In Range with Player");
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            timer += Time.deltaTime;
            //Debug.Log(timer);

            if (timer >= timeBetweenAttacks)
            {
                //Debug.Log("Attack Timer True");
                if(enemyHealth.currentHealth > 0)
                {
                    //Debug.Log("HP above 0");
                    Attack();
                }
            }
        }
    }

    void Attack ()
    {
        timer = 0f;
        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
