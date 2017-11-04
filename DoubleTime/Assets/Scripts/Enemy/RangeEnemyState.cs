using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyState : EnemyStates {

    private float internalTimer;

    // Aggresive State //
    protected override void Aggresive()
    {
        if (lookAtPlayer)
        {
            nav.updateRotation = false;

            float step = rotateSpeed * Time.deltaTime;
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 lookDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        internalTimer += Time.deltaTime;

        if(internalTimer >= rePosTime)
        {
            //posFound = false;
            //state = EnemyStates.State.REPOSITION;
            RePosition();
            internalTimer = 0;
        }
    }
}
