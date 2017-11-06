using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyState : EnemyStates {

    [Header("Reposition Settings")]
    public float minDist = 5.0f;
    public float maxDist = 8.0f;
    public float rePosTime = 3.0f;
    public float reposSpeed = 10;

    private float travelDist;
    private bool posFound;
    private Vector3 nextPos;
    private int pickPos;
    private float internalTimer;

    // Aggresive State //
    protected override void Aggresive()
    {
        if (lookAtPlayer)
        {
            nav.updateRotation = false;

            float step = rotateSpeed * Time.deltaTime;
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 lookDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        internalTimer += Time.deltaTime;
        if (internalTimer >= rePosTime)
        {
            //posFound = false;
            //state = EnemyStates.State.REPOSITION;
            RePosition();
        }
    }

    public void RePosition()
    {
        //Debug.Log("Repositioning");
        if (!posFound)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

            travelDist = Random.Range(minDist, maxDist);

            if (distanceFromPlayer <= 10.0f)
            {
                pickPos = Random.Range(1, 3);

                //Debug.Log(pickPos);

                if (pickPos == 1)
                {
                    nextPos = transform.position + transform.right * travelDist;
                }
                else
                {
                    nextPos = transform.position - transform.right * travelDist;
                }
            }
            else
            {
                nextPos = transform.position + transform.forward * travelDist;
            }

            posFound = true;
        }
        else
        {
            //Debug.Log(posFound);

            NavMeshHit hit;
            NavMesh.SamplePosition(nextPos, out hit, travelDist, NavMesh.AllAreas);
            Vector3 finalPos = hit.position;

            //Debug.Log(Vector3.Distance(transform.position, hit.position));

            //Debug.Log(finalPos);

            if (Vector3.Distance(transform.position, hit.position) > travelDist + 1.0f)
            {
                //Debug.Log("Invalid Pos");

                // Find another pos
                posFound = false;

                internalTimer = rePosTime;
            }
            else
            {
                //Debug.Log("Moving");

                nav.speed = reposSpeed;

                nav.SetDestination(hit.position);

                if (Vector3.Distance(transform.position, nav.destination) <= 2.5f)
                {
                    //Debug.Log("Destination Reached");
                    //state = EnemyStates.State.AGGRO;
                    posFound = false;

                    // Reset timer
                    internalTimer = 0;
                }
            }
        }
    }
}
