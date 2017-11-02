using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEventScript : EnemyZone {

    public GameObject asset;

    protected override void DestroyAreaZone()
    {
        base.DestroyAreaZone();

        asset.SetActive(true);
    }
}
