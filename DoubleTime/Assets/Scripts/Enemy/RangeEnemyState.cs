using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyState : EnemyStates {

    [Header("Reposition Settings")]
    public float minDist = 5.0f;
    public float maxDist = 8.0f;
    public float minRate = 3.0f;
    public float maxRate = 8.0f;
    public float reposSpeed = 10;

    private float reposTime;
    private float travelDist;
    private bool posFound;
    private Vector3 nextPos;
    private int pickPos;
    private float internalTimer;
    private bool timeGotten;

    private void OnEnable()
    {
        RePosition();
    }

    // Aggresive State //
    protected override void Aggresive()
    {
        if (lookAtPlayer)
        {
            nav.updateRotation = false;

            Vector3 targetDir = player.transform.position - transform.position;
            targetDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotateTime);
        }

        // Calculate distance and decide which action to take
        CalculateNewPosition();

        // Get Random Time
        GetTime();

        internalTimer += Time.deltaTime;

        if (internalTimer >= reposTime)
        {
            //Debug.Log("Repositioning");

            // Move to new Pos
            RePosition();
        }
    }

    public void GetTime()
    {
        if (!timeGotten)
        {
            travelDist = Random.Range(minDist, maxDist);
            reposTime = Random.Range(minRate, maxRate);
            timeGotten = true;
        }
    }

    public void CalculateNewPosition()
    {
        // Calculate pos to travel to
        Vector3 dir = transform.position - player.transform.position;

        //Debug.DrawRay(transform.position, transform.right * travelDist / 2, Color.red);
        //Debug.DrawRay(transform.position, -transform.right * travelDist / 2, Color.red);
        //Debug.DrawRay(transform.position, -transform.forward * travelDist / 2, Color.red);

        //float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log(distanceFromPlayer);

        if (dir.sqrMagnitude < 10.0f * 10.0f)
        {
            //Debug.Log("In Dist");

            RaycastHit rightHit;
            RaycastHit leftHit;

            if (Physics.Raycast(transform.position, transform.right, out rightHit, travelDist / 2, 11))
            {
                if (rightHit.collider.gameObject.layer == 11)
                {
                    //Debug.Log("Right Obstructed");
                    nextPos = transform.position - transform.right * travelDist;
                }
            }
            else if (Physics.Raycast(transform.position, -transform.right, out leftHit, travelDist / 2, 11))
            {
                if (leftHit.collider.gameObject.layer == 11)
                {
                    //Debug.Log("Left Obstructed");
                    nextPos = transform.position + transform.right * travelDist;
                }
            }
            else if (dir.sqrMagnitude < 5.0f * 5.0f)
            {
                RaycastHit behindHit;

                if (Physics.Raycast(transform.position, -transform.forward, out behindHit))
                {
                    if (behindHit.collider.gameObject.layer == 11)
                    {
                        //Debug.Log("Back Obstructed");
                        return;
                    }
                }
                else
                {
                    nextPos = transform.position - transform.forward * travelDist;
                }
            }
            else
            {
                //Debug.Log("UnObstructed");

                pickPos = Random.Range(1, 3);

                if (pickPos == 1)
                {
                    nextPos = transform.position + transform.right * travelDist;
                }
                else
                {
                    nextPos = transform.position - transform.right * travelDist;
                }
            }
        }
        else
        {
            //Debug.Log("Too Far");
            nextPos = transform.position + transform.forward * travelDist;
        }
    }

    public void RePosition()
    {
        //Debug.Log(posFound);

        NavMeshHit hit;
        NavMesh.SamplePosition(nextPos, out hit, travelDist, NavMesh.AllAreas);
        Vector3 finalPos = hit.position;

        //Debug.Log(Vector3.Distance(transform.position, hit.position));

        //Debug.Log(finalPos);

        nav.speed = reposSpeed;

        nav.SetDestination(hit.position);

        timeGotten = false;
        internalTimer = 0;
    }
}
