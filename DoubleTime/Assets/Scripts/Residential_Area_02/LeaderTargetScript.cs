using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderTargetScript : MonoBehaviour {

    public GameObject leaderEventManager;

    private LeaderEventScript eventScript;
    private void Awake()
    {
        eventScript = leaderEventManager.GetComponent<LeaderEventScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.gameObject == eventScript.leader)
        {
            // Trigger Event Over
            eventScript.EventOver();

            // Destroy Event Objects
            Destroy(other.gameObject);
            Destroy(gameObject);
            Destroy(leaderEventManager);
        }
    }
}
