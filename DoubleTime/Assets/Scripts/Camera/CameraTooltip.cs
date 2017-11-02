using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ToolTipScript))]
public class CameraTooltip : PanCameraScript {

    private ToolTipScript toolTipScript;

	// Use this for initialization
	void Awake () {
        toolTipScript.GetComponent<ToolTipScript>();
	}

    protected override void BackToOrigin()
    {
        base.BackToOrigin();
    }
}
