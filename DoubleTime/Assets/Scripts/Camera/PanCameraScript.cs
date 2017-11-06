using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PanCameraScript : MonoBehaviour {

    [Header("Camera Pan Settings")]
    public Camera mainCamera;
    public GameObject panTarget;
    public float panTime;

    private PlayerCamera playerCam;
    private PlayerMovement playerMove;
    private WeaponInventory weapInven;
    private SlowTimeScript slowTime;
    private PlayerMelee playerMelee;
    private PlayerPause playerPause;
    private GameObject player;

    private bool panToTarget;

    [Header("Boundaries")]
    public List<GameObject> activateBoundaries = new List<GameObject>();

    [Header("Display Tool Tip Settings")]
    public bool toolTip;
    public float toolTipTime;

    [Header("Debug")]
    public bool debug;
    public Text debugText;
    private float debugTimer;

    public bool destroyObject { get; set; }
    private float objectRemovalTime = 1;
    public bool destroyScript { get; set; }
    private float scriptRemovalTime = 1;

    [Header("On Camera Pan Fin")]
    public bool enableSlowTime = true;

    private void Awake()
    {
        destroyObject = true;
        destroyScript = true;

        if(debugText != null)
        {
            if (debug)
            {
                debugTimer = panTime;
                debugText.gameObject.SetActive(true);
            }
            else
            {
                debugText.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;

            playerCam = mainCamera.GetComponent<PlayerCamera>();
            playerMove = other.GetComponent<PlayerMovement>();
            weapInven = other.GetComponent<WeaponInventory>();
            slowTime = other.GetComponent<SlowTimeScript>();
            playerMelee = other.GetComponent<PlayerMelee>();
            playerPause = other.GetComponent<PlayerPause>();

            panToTarget = true;
            Invoke("TogglePanToTarget", panTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            DebugText();

            if (panToTarget)
            {
                CameraPan();
            }
            else
            {
                BackToOrigin();
            }
        }
    }

    private void TogglePanToTarget()
    {
        panToTarget = !panToTarget;
    }

    // Pans the Camera to targetted Position, also disables movement
    private void CameraPan()
    {
        playerCam.target = panTarget.transform;

        playerMove.horizontal = 0;
        playerMove.vertical = 0;
        playerMove.enabled = false;

        slowTime.enabled = false;
        playerMelee.enabled = false;
        playerPause.enabled = false;

        // Disable all scripts on weapons //
        for (int i = 0; i < weapInven.weaponInventory.Count; i++)
        {
            weapInven.weaponInventory[i].GetComponent<MonoBehaviour>().enabled = false;
        }

        weapInven.enabled = false;
    }

    // Pans the camera back from target pos, re-enables movement
    protected virtual void BackToOrigin()
    {
        playerCam.target = player.transform;
        playerMove.enabled = true;
        playerMelee.enabled = true;
        playerPause.enabled = true;

        if (enableSlowTime)
        {
            slowTime.enabled = true;
        }

        if (!weapInven.enabled)
        {
            weapInven.enabled = true;
        }

        // Enable all scripts on weapon //
        for (int i = 0; i < weapInven.weaponInventory.Count; i++)
        {
            weapInven.weaponInventory[i].GetComponent<MonoBehaviour>().enabled = true;
        }

        // Display Tool tip
        if (toolTip)
        {
            ShowToolTip();
        }

        // Activate any boundary
        ActivateBoundaries();

        // Check for removal
        RemoveObject(); 
        RemoveScript();
    }

    // Removes gameobject tied to this script
    private void RemoveObject()
    {
        if (destroyObject)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, objectRemovalTime);
        }
    }

    // Removes this script
    private void RemoveScript()
    {
        if (destroyScript)
        {
            Destroy(GetComponent<PanCameraScript>(), scriptRemovalTime);
        }
    }

    // Activates All Boundaries in List in there are any
    private void ActivateBoundaries()
    {
        if(activateBoundaries.Count > 0)
        {
            foreach (GameObject boundary in activateBoundaries)
            {
                if (boundary != null)
                {
                    boundary.SetActive(true);
                }
            }
        }
    }

    // Displays tool tip if there is any
    private void ShowToolTip()
    {
        if(GetComponent<ToolTipScript>() != null)
        {
            ToolTipScript toolTipScript = GetComponent<ToolTipScript>();

            toolTipScript.DisplayToolTip(toolTipTime);

            objectRemovalTime += toolTipTime;
        }
    }

    // Show debug text for Pan Time
    private void DebugText()
    {
        if (debug)
        {
            if(debugTimer > 0)
            {
                debugText.gameObject.SetActive(true);
                debugTimer -= Time.deltaTime;
                debugText.text = "Waiting: " + debugTimer.ToString();
                //Debug.Log(debugTimer);
            }
            else
            {
                debugTimer = 0;
                debugText.gameObject.SetActive(false);
            }
        }
    }
}
