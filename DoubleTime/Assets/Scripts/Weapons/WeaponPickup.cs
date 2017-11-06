using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToolTipScript))]
public class WeaponPickup : MonoBehaviour {

    public KeyCode weaponKeyCode;

    public GameObject player;

    [Header("Weapon Tool Tip")]
    public float displayTime;

    public void WeaponAdd()
    {
        // Add corresponding KeyCode
        if (player.GetComponent<WeaponInventory>() != null)
        {
            player.GetComponent<WeaponInventory>().keyCodes.Add(weaponKeyCode);
        }

        // Update weapon Tool bar UI
        player.GetComponent<WeaponInventory>().UpdateWeaponToolbar();

        // Show and hide tooltip after display time
        GetComponent<ToolTipScript>().DisplayToolTip(displayTime);
    }
}
