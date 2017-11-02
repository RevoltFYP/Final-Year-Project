using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour {

    public enum State
    {
        PATROL,
        AGGRO,
        DEAGGRO,
        NONE
    }

    public State state;

    [Header("Patrol Properties")]
    // Variables for Patrolling
    public float patrolSpeed;
    public float waitTime;
    public float lookSpeed = 3;
    public GameObject[] wayPoints;
    private int wayPointCounter = 0;
    private Quaternion targetRot;
    private float internalWaitTimer;

    [Header("Aggro Properties")]
    // Variables for Aggro
    public bool lookAtPlayer;
    public float aggroSpeed;
    public float rotateSpeed;

    public Transform player { get; set; }
    private EnemyHealth enemyHealth;
    private float velocity;
    public NavMeshAgent nav { get; set; }

    private Vector3 startPos;

    void Awake ()
    {
        startPos = transform.position;
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        //internalWaitTimer = waitTime;
    }

    private void Update()
    {
        // Controls States //
        if (!enemyHealth.isDead)
        {
            Debug.Log(internalWaitTimer);
            switch (state)
            {
                case State.NONE:
                    None();
                    break;
                case State.PATROL:
                    Patrol();
                    break;
                case State.AGGRO:
                    Aggresive();
                    break;
                case State.DEAGGRO:
                    DeAggro();
                    break;
            }
        }
    }

    // Patrol State //
    protected void Patrol()
    {
        internalWaitTimer += Time.deltaTime;

        if (internalWaitTimer >= waitTime)
        {
            if (wayPoints.Length > 0)
            {
                // Slowly increases speed to patrol speed //
                if (nav.speed != patrolSpeed)
                {
                    nav.speed = patrolSpeed;
                }

                // too far away from waypoint
                if (Vector3.Distance(transform.position, wayPoints[wayPointCounter].transform.position) > 1)
                {
                    nav.SetDestination(wayPoints[wayPointCounter].transform.position); // go to waypoint

                    // Rotate towards waypoint direction
                    Vector3 dir = wayPoints[wayPointCounter].transform.position - transform.position;
                    dir.y = 0;

                    float angle = Vector3.Angle(transform.forward, dir);

                    if (angle > 0.1f)
                    {
                        targetRot = Quaternion.LookRotation(dir);
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * lookSpeed);
                    }
                }
                // Reset way point counter
                else if (Vector3.Distance(transform.position, wayPoints[wayPointCounter].transform.position) <= 1)
                {
                    // reset waypoint back to beginning (loop patrol)
                    if (wayPointCounter >= wayPoints.Length)
                    {
                        wayPointCounter = 0;
                    }
                    else
                    {
                        // Goes to next way point
                        wayPointCounter += 1;
                    }
                }
            }
            internalWaitTimer = 0;
        }
    }

    // Null State
    private void None()
    {
        return;
    }

    // Aggresive State //
    protected virtual void Aggresive()
    {
        if (lookAtPlayer)
        {
            //transform.LookAt(player.position);
            float step = rotateSpeed * Time.deltaTime;
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 lookDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Slowly increases speed to aggro speed
        nav.speed = aggroSpeed;

        nav.SetDestination(player.position);
    }

    private void DeAggro()
    {
        nav.SetDestination(startPos);

        if(Vector3.Distance(transform.position, startPos) <= 1)
        {
            state = EnemyStates.State.NONE;
        }
    }

    // Agent Functions //
    public void MoveAgent(Vector3 location, float moveSpeed)
    {
        nav.speed = moveSpeed;
        nav.SetDestination(location);
    }

    public void StopAgent(bool stop)
    {
        if (!enemyHealth.isDead)
        {
            nav.isStopped = stop;
        }
    }

    public void ToPatrol()
    {
        state = EnemyStates.State.PATROL;
    }

    public void ToAggro(Transform targetTransform)
    {
        player = targetTransform;
        state = EnemyStates.State.AGGRO;
    }

}
