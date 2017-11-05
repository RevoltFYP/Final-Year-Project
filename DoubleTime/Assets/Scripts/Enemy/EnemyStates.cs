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
        RANDOMPATROL
    }

    public State state;
    [Header("Patrol Properties")]
    public float waitTime;
    private float internalWaitTimer;

    [Header("Waypoint Patrol Properties")]
    // Variables for Patrolling
    public float patrolSpeed;
    public float lookSpeed = 3;
    public GameObject[] wayPoints;
    private int wayPointCounter = 0;
    private Quaternion targetRot;

    [Header("Area Patrol Properties")]
    public bool displayMoveArea;
    public float moveArea;
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
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (displayMoveArea)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(aggroZone.transform.position + Random.insideUnitSphere, moveArea);
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
                Vector3 newPos = new Vector3(randomPos.x, transform.position.y, randomPos.z);

                NavMeshHit hit;
                NavMesh.SamplePosition(newPos, out hit, moveArea, NavMesh.AllAreas);
                Vector3 finalPos = hit.position;

                nav.SetDestination(newPos);

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
