using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCameraAggro : PanCameraScript {

    [Header("Areas to Aggro")]
    public List<GameObject> areas = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject area in areas)
        {
            area.SetActive(false);
        }
    }

    protected override void BackToOrigin()
    {
        base.BackToOrigin();

        foreach(GameObject area in areas)
        {
            area.SetActive(true);
        }
    }

}
