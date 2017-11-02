using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(ToolTipScript))]
public class EnemyToolTip : MonoBehaviour {

    [Range(0, 100)] public float healthPercent;
    public float displayTime;

    private EnemyHealth enemyHealth;
    private ToolTipScript toolTipscript;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        toolTipscript = GetComponent<ToolTipScript>();
    }
 
    private void Update()
    {
        if(enemyHealth.currentHealth <= enemyHealth.startingHealth * (healthPercent / 100))
        {
            toolTipscript.DisplayToolTip(displayTime);
        }
    }
}
