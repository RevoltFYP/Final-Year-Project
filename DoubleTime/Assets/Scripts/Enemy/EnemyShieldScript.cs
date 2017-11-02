using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldScript : MonoBehaviour {

    public int playerBulletLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerBulletLayer)
        {
            Debug.Log("true");
            other.GetComponent<ProjectileBase>().Destroy();
        }
    }
}
