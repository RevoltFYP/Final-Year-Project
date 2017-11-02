using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ToolTipScript))]

public class TutorialEndingScript : MonoBehaviour {

    public GameObject enemyZone;

    private PanCameraScript cameraPan;

    private void Awake()
    {
        cameraPan = GetComponent<PanCameraScript>();
    }

    private void Start()
    {
        cameraPan.destroyObject = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("ActivateEnemies", cameraPan.panTime);
        }
    }

    // Aggro enemies
    private void ActivateEnemies()
    {
        enemyZone.SetActive(true);
    }
}
