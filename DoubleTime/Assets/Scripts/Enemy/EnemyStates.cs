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
        WANDERING
    }

    public State state;

    [Header("Patrol Properties")]
    // Variables for Patrolling
    public float waitTime;
    private float internalWaitTimer;
    public float patrolSpeed;
    public float lookSpeed = 3;
    public GameObject[] wayPoints;
    private int wayPointCounter = 0;
    private Quaternion targetRot;

    [Header("Wandering Properties")]
    public float wanderSpeed;
    public bool displayMoveArea;
    public float randomMinTime;
    public float randomMaxTime;
    public GameObject aggroZone;
    public Vector3 wanderArea;
    public Vector3 wanderOffset;
    private float randomTime = 0;

    [Header("Aggro Properties")]
    public bool lookAtPlayer;
    public float aggroSpeed;
    [Range(0,1)] public float rotateTime;

    [Header("DeAggro Properties (Obseleate)")]
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
                case State.WANDERING:
                    Wander();
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (displayMoveArea)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(aggroZone.transform.position + wanderOffset, wanderArea);
        }
    }

    //Random Patrol State //
    public void Wander()
    {
        if(wanderArea != Vector3.zero)
        {
            nav.updateRotation = true;
            nav.speed = wanderSpeed;

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
                    Vector3 randomPos = aggroZone.transform.position + new Vector3(Random.Range(-wanderArea.x / 2, wanderArea.x / 2), 0f, Random.Range(-wanderArea.z / 2, wanderArea.z / 2)) + wanderOffset;
                    Vector3 newPos = new Vector3(randomPos.x, transform.position.y, randomPos.z);

                    NavMeshHit hit;
                    NavMesh.SamplePosition(newPos, out hit, Vector3.SqrMagnitude(wanderArea), NavMesh.AllAreas);
                    Vector3 finalPos = hit.position;

                    nav.SetDestination(newPos);

                    internalWaitTimer = 0;
                    randomTime = 0;
                }
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
                nav.speed = patrolSpeed;

                Vector3 distance = transform.position - wayPoints[wayPointCounter].transform.position;

                // too far away from waypoint
                if (distance.sqrMagnitude >= 2 * 2)
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
                else if (distance.sqrMagnitude <= 2 * 2)
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
        Vector3 targetDir = player.transform.position - transform.position;

        if (lookAtPlayer)
        {
            nav.updateRotation = false;

            targetDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotateTime);
        }

        // Increases speed to aggro speed
        nav.speed = aggroSpeed;

        if(targetDir.sqrMagnitude > 3 * 3)
        {
            nav.SetDestination(player.position);
        }
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
