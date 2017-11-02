using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;            // The position that the camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    public Vector3 offset;             // The initial offset from the target.

    [Header("Boundaries")]
    public bool clamp;
    public Vector3 minClamp;
    public Vector3 maxClamp;

    void LateUpdate ()
    {
        // Create a position the camera is aiming for based on the offset from the target.
        Vector3 targetCameraPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, smoothing * Time.unscaledDeltaTime);

        if (clamp)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minClamp.x, maxClamp.x),
                targetCameraPos.y,
                Mathf.Clamp(transform.position.z, minClamp.z, maxClamp.z)
                );
        }
    }
}
