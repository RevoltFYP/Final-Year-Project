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
    private ToolTipScript toolTipscript;

    private WeaponInventory weapInven;

    private void Awake()
    {
        toolTipscript = GetComponent<ToolTipScript>();

        weapInven = player.GetComponent<WeaponInventory>();
    }

    public void WeaponAdd()
    {
        // Add corresponding KeyCode
        if (weapInven.keyCodes != null)
        {
            weapInven.keyCodes.Add(weaponKeyCode);
        }

        // Update weapon Tool bar UI
        weapInven.UpdateWeaponToolbar();

        // Show and hide tooltip after display time
        toolTipscript.DisplayToolTip(displayTime);
    }
}
