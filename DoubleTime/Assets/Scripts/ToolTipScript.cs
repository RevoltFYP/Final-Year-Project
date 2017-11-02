using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScript : MonoBehaviour {

    [Header("Weapon Tool Tip")]
    public GameObject toolTip;
    public Text toolTipTxt;
    [TextArea] public string toolTipText;

    public void DisplayToolTip(float displayTime)
    {
        if(toolTipText != string.Empty)
        {
            ShowToolTip();
            Invoke("HideToolTip", displayTime);
        }
    }

    public void ShowToolTip()
    {
        toolTipTxt.text = toolTipText;
        toolTip.SetActive(true);
    }

    public void HideToolTip()
    {
        toolTipTxt.text = string.Empty;
        toolTip.SetActive(false);
    }
}
