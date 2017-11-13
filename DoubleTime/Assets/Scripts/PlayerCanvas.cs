using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour {

    public Vector3 offset;
    public Transform target;
    public bool lockAxis;
    private Quaternion currRotation;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update() {

        transform.position = player.transform.position + offset;

        LockAxis();
        LookAt(target);
    }

    public void LookAt(Transform target)
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void LockAxis()
    {
        if (lockAxis)
        {
            if (transform.rotation != currRotation)
            {
                transform.rotation = currRotation;
            }
        }
    }
}
