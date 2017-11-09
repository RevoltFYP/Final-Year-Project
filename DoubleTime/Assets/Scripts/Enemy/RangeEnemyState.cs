using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyState : EnemyStates {

    [Header("Reposition Settings")]
    public float minDist = 5.0f;
    public float maxDist = 8.0f;
    public float rePosRate = 3.0f;
    public float reposSpeed = 10;

    private float heightMultiplyer = 0.6f;
    private float travelDist;
    private bool posFound;
    private Vector3 nextPos;
    private int pickPos;
    private float internalTimer;

    private void Start()
    {
        internalTimer = rePosRate;
    }

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

        CalculateNewPosition();

        internalTimer += Time.deltaTime;

        if (internalTimer >= rePosRate)
        {
            //Debug.Log("Repositioning");

            // Move to new Pos
            RePosition();
        }
    }

    public void CalculateNewPosition()
    {
        // Calculate pos to travel to
        Vector3 dir = transform.position - player.transform.position;
        travelDist = Random.Range(minDist, maxDist);

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
                if (rightHit.collider.gameObject.tag == "Building")
                {
                    //Debug.Log("Right Obstructed");
                    nextPos = transform.position - transform.right * travelDist;
                }
            }
            else if (Physics.Raycast(transform.position, -transform.right, out leftHit, travelDist / 2, 11))
            {
                if (leftHit.collider.gameObject.tag == "Building")
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
                    if (behindHit.collider.gameObject.tag == "Building")
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

        internalTimer = 0;
    }

    /*public void RePosition()
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
    }*/
}
