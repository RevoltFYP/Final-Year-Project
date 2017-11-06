using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCollider : MonoBehaviour {

    public int damagePerAttack = 20;

	void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "MeleeEnemy" || other.gameObject.tag == "RangeEnemy")
        {
           EnemyHealth meleeEnemyHealth = other.GetComponent<EnemyHealth>();
           meleeEnemyHealth.TakeDamage(damagePerAttack);
            //Debug.Log("Hit Melee");
        }
    }
}
