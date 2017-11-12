using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {
    public float time;

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.RotateAround(transform.position, Vector3.up, 360 * Time.deltaTime / time);
        }
    }
}
