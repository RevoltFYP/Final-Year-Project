using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToolTipScript))]
public class SlowTimeEvent : MonoBehaviour {

    public GameObject slowPrompt;


    public GameObject enemyZone;
    private PanCameraScript cameraPan;
    private ToolTipScript toolTipScript;

    private void Awake()
    {
        toolTipScript = GetComponent<ToolTipScript>();
        cameraPan = GetComponent<PanCameraScript>();
    }

    // In start because destroy object is set in Awake in Pan Camera
    private void Start()
    {
        cameraPan.destroyObject = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("ShowSlowTimePrompt", cameraPan.panTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SlowTimeScript slowTime = other.GetComponent<SlowTimeScript>();
            PlayerMovement playerMove = other.GetComponent<PlayerMovement>();

            // Disable movement
            playerMove.enabled = false;

            // Enable slow time script
            slowTime.enabled = true;

            // Begin event to check for slow time
            StartCoroutine(CheckSlow(slowTime, playerMove));
        }
    }

    // Slow Time Event
    private IEnumerator CheckSlow(SlowTimeScript slowTime, PlayerMovement playerMove)
    {
        if (Input.GetKeyDown(slowTime.slowDownKey))
        {
            // Hide slow prompt
            HideSlowTimePrompt();

            // Enable slow time tool tip
            toolTipScript.DisplayToolTip(cameraPan.panTime);

            // Allow player movement
            playerMove.enabled = true;

            // Enemies to aggro
            enemyZone.SetActive(true);

            // Destroy this gameObject
            Destroy(gameObject);
        }
        yield return null;
    }

    // Display prompt
    private void ShowSlowTimePrompt()
    {
        slowPrompt.SetActive(true);
    }

    // Hide prompt
    private void HideSlowTimePrompt()
    {
        slowPrompt.SetActive(false);
    }
}
