using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToolTipScript))]
public class TargetEvent : MonoBehaviour {

    public GameObject target;
    public float displayTime;

    [TextArea] public string aliveText;
    [TextArea] public string deadText;

    private ToolTipScript toolTipScript;
	// Use this for initialization
	void Awake () {
        toolTipScript = GetComponent<ToolTipScript>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(target != null)
            {
                toolTipScript.toolTipText = aliveText;
            }
            else
            {
                toolTipScript.toolTipText = deadText;
            }

            toolTipScript.DisplayToolTip(displayTime);

            // Disable script
            this.enabled = false;

            // Destroy after use
            Destroy(this, displayTime);
            Destroy(toolTipScript, displayTime);
        }
    }
}
