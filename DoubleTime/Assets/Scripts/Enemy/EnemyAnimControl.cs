using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimControl : MonoBehaviour {

    public float runDampTime = 0.1f;
    private Animator anim;

    private EnemyStates enemyState;
    private EnemyHealth enemHealth;

    //private Transform cam;
    //private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;

    private float forwardAmount;
    private float rotateAmount;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        // Get main camera on scene
        //cam = Camera.main.transform;

        enemyState = transform.parent.GetComponent<EnemyStates>();
        enemHealth = transform.parent.GetComponent<EnemyHealth>();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (cam != null)
        {
            // Sets scale of camera
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;

            // Gets direction of camera
            move = enemyState.nav.velocity.z * camForward + enemyState.nav.velocity.x * cam.right;
        }
        else
        {
            // Gets direction of camera
            move = enemyState.nav.velocity.z  * Vector3.forward + enemyState.nav.velocity.x * Vector3.right;
        }*/

        if (!enemHealth.isDead)
        {
            move = new Vector3(enemyState.nav.velocity.x, 0f, enemyState.nav.velocity.z);
            Move(move);
        }
        else
        {
            OnDeath();
        }
    }

    private void Move(Vector3 move)
    {
        // Get a small value
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        moveInput = move;

        // Converts world moveInput to local ( takes only the x and z values )
        ConvertMoveInput();

        // Updates animation using converted values
        UpdateAnimator();
    }

    //Converts moveInput from World to Local
    private void ConvertMoveInput()
    {
        // Takes a direction and converts from World to Local
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        rotateAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    // Update animation with new direction
    private void UpdateAnimator()
    {
        anim.SetFloat("VelX", rotateAmount, runDampTime, Time.deltaTime);
        anim.SetFloat("VelY", forwardAmount, runDampTime, Time.deltaTime);
    }
    private void OnDeath()
    {
        rotateAmount = 0;
        forwardAmount = 0;

        //Debug.Log("Death");
    }
}
