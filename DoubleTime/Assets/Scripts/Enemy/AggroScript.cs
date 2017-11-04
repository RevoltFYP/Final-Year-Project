using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour {

    private EnemyStates enemyStates;

    private float internalTimer;

    private void Awake()
    {
        enemyStates = transform.parent.GetComponent<EnemyStates>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("Aggro");

            if (IsInvoking("GoToDeAggro"))
            {
                //Debug.Log("De Aggro");

                // Cancels the invoke if player steps in to aggro radius again
                CancelInvoke("GoToDeAggro"); 
            }

            enemyStates.state = EnemyStates.State.AGGRO;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("GoToDeAggro", enemyStates.deAggroTime); // reverts back to original pos if player leaves aggro radius for deAggroTime
        }
    }

    protected virtual void GoToDeAggro()
    {
        //Debug.Log("De Aggro");
        //enemyStates.state = EnemyStates.State.DEAGGRO;
    }
}
