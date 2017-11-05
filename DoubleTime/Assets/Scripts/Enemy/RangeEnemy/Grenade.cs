using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    [Header("Grenade Stats")]
    public float waitTime = 5f;
    public float radius = 5f;
    public float explosionForce = 100f;
    public float explosionUpwardsForce = 0;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("Blast",waitTime);
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    IEnumerator Blast()
    {
        //waits for a certain interval of time
        yield return new WaitForSeconds(waitTime);
        
        //get all the collider within the range
        Collider[] objects = Physics.OverlapSphere(this.transform.position, radius);

        foreach(Collider gameobj in objects)
        {
            //Check if the gameobject have rigidbody
            if(gameobj.gameObject.GetComponent<Rigidbody>() != null)
            {
                gameobj.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, radius, explosionUpwardsForce);
            }
        }
    }
}
