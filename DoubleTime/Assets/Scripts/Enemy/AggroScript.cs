using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour {

    public EnemyStates enemyState;
    public float deAggroTime;

    protected virtual void OnTriggerEnter(Collider other)
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

            enemyState.ToAggro(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("GoToDeAggro", deAggroTime); // reverts back to patrol if player leaves aggro radius for deAggroTime
        }
    }

    protected virtual void GoToDeAggro()
    {
        //Debug.Log("De Aggro");
        enemyState.state = EnemyStates.State.DEAGGRO;
    }
}
