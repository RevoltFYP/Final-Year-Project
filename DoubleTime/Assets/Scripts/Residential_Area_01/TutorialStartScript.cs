using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ToolTipScript))]
public class TutorialStartScript : MonoBehaviour {

    private ToolTipScript toolTipScript;
    public float displayTime;

    private void Awake()
    {
        toolTipScript = GetComponent<ToolTipScript>();
    }

    private void Start()
    {
        toolTipScript.DisplayToolTip(displayTime);
        Destroy(gameObject, displayTime);
    }
}
