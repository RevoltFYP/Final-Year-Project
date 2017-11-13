using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    [Header("Move Speed")]
    public float   playerSpeed = 5f;               // Player speed

    private Vector3 movement;                       // The Vector to store the direction of the player's movement
    private Rigidbody playerRigibody;                 // Reference to the player's rigidbody
    private int floorMask;                      // A layer mask so that a raycast can be cast just at the gameobject on the floor layer
    private float cameraRayLength = 100f;         // The Length of the ray from the camera into the scene

    public float horizontal { get; set; }
    public float vertical { get; set; }

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        playerRigibody  = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3 (horizontal, 0f, vertical);

        // Normalized move to prevent faster diagonal movement
		transform.position += movement * Time.deltaTime * playerSpeed * 1 / Time.timeScale;

        // Turn the player ro face the mouse cursor
        Turning();
    }

    void Turning ()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit cariable to store information about what was the hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer.
        if (Physics.Raycast (cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            //lookPos = floorHit.point;
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playertoMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playertoMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playertoMouse);

            // Set the player's rotation to this new rotation.
            playerRigibody.MoveRotation(newRotation);
        }
    }
}
