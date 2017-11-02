using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStates))]
public class ChargerScript : MonoBehaviour {

    [Range(0,5)] public float chargeForce;
    public float chargeRange;
    public float chargeCoolDown;
    public float chargeBuffer;

    private RaycastHit hit;
    public float heightMultiplyer = 0.6f;

    private bool canCharge = true;
    private NavMeshAgent agent;
    private Transform playerLastPos;
    private Rigidbody rb;
    private Vector3 baseVelocity;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        baseVelocity = rb.velocity;
	}

    // Update is called once per frame
    void FixedUpdate ()
    {
        //Debug.Log(canCharge);
        //Debug.Log(timer);
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplyer, transform.forward, out hit, chargeRange))
        {
            if(hit.collider.gameObject.tag == "Player" && canCharge) // if ai sees player
            {
                playerLastPos = hit.collider.gameObject.transform;
                StartCoroutine(Charge(playerLastPos));
            }
        }

        Debug.DrawRay(transform.position + Vector3.up * heightMultiplyer, transform.forward * chargeRange, Color.green);
    }

    // Charge Behaviour //
    private IEnumerator Charge(Transform chargePos)
    {
        // Stop agent
        agent.isStopped = true;

        // Wait for X amount of seconds
        yield return new WaitForSeconds(chargeBuffer);

        // Charge towards player last known pos
        rb.AddForce(transform.forward * chargeForce, ForceMode.Impulse);

        // Reset cooldowns
        canCharge = false;
        Invoke("CoolDown", chargeCoolDown);

        yield return new WaitForSeconds(chargeBuffer);

        // Resume movement of agent
        agent.isStopped = false;
    }

    private void CoolDown()
    {
        canCharge = true;
        rb.velocity = baseVelocity;
    }
}
