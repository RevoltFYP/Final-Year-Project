using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownScript : MonoBehaviour {

    private Quaternion currRotation;

    private void Awake()
    {
        currRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        if(transform.rotation != currRotation)
        {
            transform.rotation = currRotation;
        }
    }
}
