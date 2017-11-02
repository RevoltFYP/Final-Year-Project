using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimControl : MonoBehaviour {

    public float runDampTime = 0.1f;
    private PlayerMovement playerMove;

    private Animator anim;

    private Transform cam;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;

    private float forwardAmount;
    private float rotateAmount;

    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();

        // Get main camera on scene
        cam = Camera.main.transform;

        // Get player move script reference from Player Parent
        playerMove = transform.parent.GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cam != null)
        {
            // Sets scale of camera
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;

            // Gets direction of camera
            move = playerMove.vertical * camForward + playerMove.horizontal * cam.right;
        }
        else
        {
            // Gets direction of camera
            move = playerMove.vertical * Vector3.forward + playerMove.horizontal * Vector3.right;
        }

        // Syncs character direction to correct animation
        Move(move);
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
}
