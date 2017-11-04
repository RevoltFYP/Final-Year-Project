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
        NONE,
        RANDOMPATROL,
        REPOSITION
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

    [Header("Random Patrol Properties")]
    public float moveArea;
    private Vector3 newPos;
    public float randomMinTime;
    public float randomMaxTime;
    public GameObject aggroZone;
    private float randomTime = 0;

    [Header("Aggro Properties")]
    // Variables for Aggro
    public bool lookAtPlayer;
    public float aggroSpeed;
    public float rotateSpeed;

    [Header("DeAggro Properties")]
    public float deAggroTime;

    [Header("Reposition Settings")]
    public float distance = 5.0f;
    public float rePosTime = 3.0f;
    public bool posFound { get; set; }
    private Vector3 nextPos;
    private int pickPos;

    public Transform player { get; set; }
    private EnemyHealth enemyHealth;
    private float velocity;
    public NavMeshAgent nav { get; set; }

    private Vector3 startPos;

    void Awake ()
    {
        foreach(GameObject wayPoint in wayPoints)
        {
            if (wayPoint.GetComponent<Renderer>().enabled)
            {
                wayPoint.GetComponent<Renderer>().enabled = false;
            }
        }

        startPos = transform.position;
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        internalWaitTimer = waitTime;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Controls States //
        if (!enemyHealth.isDead)
        {
            //Debug.Log(state);
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
                case State.RANDOMPATROL:
                    RandomPatrol();
                    break;
                case State.REPOSITION:
                    RePosition();
                    break;
            }
        }
    }

    //Random Patrol State //
    public void RandomPatrol()
    {
        nav.updateRotation = true;

        if (randomTime == 0)
        {
           randomTime = Random.Range(randomMinTime, randomMaxTime);
        }
        else
        {
            //Debug.Log(randomTime);
            internalWaitTimer += Time.deltaTime;

            if (internalWaitTimer > randomTime)
            {
                Vector3 randomPos = aggroZone.transform.position + Random.insideUnitSphere * moveArea;
                newPos = new Vector3(randomPos.x, transform.position.y, randomPos.z);

                NavMeshHit hit;
                NavMesh.SamplePosition(newPos, out hit, moveArea, NavMesh.AllAreas);
                Vector3 finalPos = hit.position;

                nav.SetDestination(finalPos);

                internalWaitTimer = 0;
                randomTime = 0;
            }
        }
    }

    // Patrol State //
    public void Patrol()
    {
        nav.updateRotation = true;

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
                if (Vector3.Distance(transform.position, wayPoints[wayPointCounter].transform.position) > 2)
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
                    // Goes to next way point
                    wayPointCounter += 1;

                    // reset waypoint back to beginning (loop patrol)
                    if (wayPointCounter >= wayPoints.Length)
                    {
                        wayPointCounter = 0;
                    }
                }
            }
            internalWaitTimer = 0;
        }
    }

    // Null State
    public void None()
    {
        return;
    }

    // Aggresive State //
    protected virtual void Aggresive()
    {
        if (lookAtPlayer)
        {
            nav.updateRotation = false;

            float step = rotateSpeed * Time.deltaTime;
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 lookDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Slowly increases speed to aggro speed
        nav.speed = aggroSpeed;

        nav.SetDestination(player.position);
    }

    public void DeAggro()
    {
        nav.updateRotation = true;

        nav.SetDestination(startPos);

        if(Vector3.Distance(transform.position, startPos) <= 1)
        {
            state = EnemyStates.State.NONE;
        }
    }

    public void RePosition()
    {
        //Debug.Log("Repositioning");
        if (!posFound)
        {
            if(Vector3.Distance(transform.position,player.position) <= 10.0f)
            {
                pickPos = Random.Range(1, 3);

                Debug.Log(pickPos);

                if (pickPos == 1)
                {
                    nextPos = transform.position + transform.right * distance;
                }
                else
                {
                    nextPos = transform.position - transform.right * distance;
                }
            }
            else
            {
                nextPos = transform.position + transform.forward * distance;
            }
            posFound = true;
        }
        else
        {
            //Debug.Log(posFound);

            NavMeshHit hit;
            NavMesh.SamplePosition(nextPos, out hit, distance, NavMesh.AllAreas);
            Vector3 finalPos = hit.position;

            //Debug.Log(Vector3.Distance(transform.position, hit.position));

            if(Vector3.Distance(transform.position, hit.position) > distance + 1.0f)
            {
                Debug.Log("Invalid Pos");

                // Find another pos
                posFound = false;
            }
            else
            {
                Debug.Log("Moving");
                nav.SetDestination(hit.position);

                if (Vector3.Distance(transform.position, nav.destination) <= 2.0f)
                {
                    //state = EnemyStates.State.AGGRO;
                    posFound = false;
                }

            }
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
}
