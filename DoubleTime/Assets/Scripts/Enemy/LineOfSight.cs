using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStates))]
public class LineOfSight : MonoBehaviour {

    public string checkTag;
    public float deAggroTime;

    [Header("Line Of Sight Properties")]
    // Variables for Line of Sight
    public float heightMultiplyer;
    public float sightDistance;

    private EnemyStates enemyState;
    // Use this for initialization
    void Start () {
        enemyState = GetComponent<EnemyStates>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Show raycast in gizmos //
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplyer, transform.forward * sightDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplyer, (transform.forward + transform.right).normalized * sightDistance, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplyer, (transform.forward - transform.right).normalized * sightDistance, Color.green);
        LOS();
	}

    // Line of Sight //
    private void LOS()
    {
        RaycastHit hit;

        // Mid Ray //
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplyer, transform.forward, out hit, sightDistance))
        {
            //Debug.Log(hit.collider.gameObject.name);

            // Only allows to go into aggro state from patrol state //
            if (hit.collider.tag == checkTag && enemyState.state == EnemyStates.State.PATROL)
            {
                // If patrol is being invoked
                if (enemyState.IsInvoking("ToPatrol"))
                {
                    // cancel it
                    enemyState.CancelInvoke("ToPatrol"); 
                }

                enemyState.state = EnemyStates.State.AGGRO;
            }
        }
        // Right Ray //
        else if (Physics.Raycast(transform.position + Vector3.up * heightMultiplyer, (transform.forward + transform.right).normalized, out hit, sightDistance))
        {
            //Debug.Log(hit.collider.gameObject.name);

            // Only allows to go into aggro state from patrol state //
            if (hit.collider.tag == checkTag && enemyState.state == EnemyStates.State.PATROL)
            {
                // If patrol is being invoked
                if (enemyState.IsInvoking("ToPatrol"))
                {
                    // cancel it
                    enemyState.CancelInvoke("ToPatrol");
                }

                enemyState.state = EnemyStates.State.AGGRO;
            }
        }
        // Left Ray //
        else if (Physics.Raycast(transform.position + Vector3.up * heightMultiplyer, (transform.forward - transform.right).normalized, out hit, sightDistance))
        {
            //Debug.Log(hit.collider.gameObject.tag);

            // Only allows to go into aggro state from patrol state //
            if (hit.collider.tag == checkTag && enemyState.state == EnemyStates.State.PATROL)
            {
                // If patrol is being invoked
                if (enemyState.IsInvoking("ToPatrol"))
                {
                    // cancel it
                    enemyState.CancelInvoke("ToPatrol");
                }

                enemyState.state = EnemyStates.State.AGGRO;
            }
        }

        else
        {
            // Only allows to go patrol from aggro state //
            if(enemyState.state == EnemyStates.State.AGGRO)
            {
                enemyState.Invoke("ToPatrol", deAggroTime);
            }
        }
    }
}
