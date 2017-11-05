using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMelee : MonoBehaviour {

    public KeyCode meleeKey;
    //public int damagePerAttack = 20;
    public float timeBetweenAttack = 0.15f;
    public bool melee { get; set; }   
    public GameObject attackHitBox;
    public Text modeText;

    private float timer;

    void Update ()
    {
        // Stop adding to timer once exceed timeBetweenAttack 
        if(timer <= timeBetweenAttack)
        {
            timer += Time.deltaTime * 1 / Time.timeScale;
        }

        if (Input.GetKeyDown(meleeKey) && timer >= timeBetweenAttack && ShootBoomarang.haveBoomarang == true)
        {
            attackHitBox.SetActive(true);
            timer = 0;
        }
        else
        {
            attackHitBox.SetActive(false);
        }

        //Debug.Log(timer);
	}
}
