using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderEventScript : MonoBehaviour {

    [Range(30,100)] public int healthPercent;
    public GameObject player;
    public GameObject leader;
    public GameObject mainCam;
    public GameObject leaderTarget;
    public float leaderStopTime;
    public float moveSpeed;

    public GameObject gate;

    public AssetEnemyZone enemyZone;
    private List<GameObject> leaderGroup = new List<GameObject>();

    private EnemyHealth leaderHealth;
    private EnemyStates leaderState;

    private PlayerMovement playerMove;
    private WeaponInventory weapInven;
    private PlayerMelee playerMelee;
    private SlowTimeScript slowTime;

    private PlayerCamera playerCamScript;
    private object enemyGroup;
    private bool leaderMoving = false;

    private void Awake()
    {
        leaderHealth = leader.GetComponent<EnemyHealth>();
        leaderState = leader.GetComponent<EnemyStates>();

        playerMove = player.GetComponent<PlayerMovement>();
        weapInven = player.GetComponent<WeaponInventory>();
        playerMelee = player.GetComponent<PlayerMelee>();
        slowTime = player.GetComponent<SlowTimeScript>();

        playerCamScript = mainCam.GetComponent<PlayerCamera>();

        leaderGroup = enemyZone.enemies;

        leaderTarget.SetActive(false);
    }

    private void Update()
    {
        //Debug.Log("Running");

        // Remove destroyed enemies
        RemainingEnemies(leaderGroup);

        // When leader health below 10%
        if (leaderHealth.currentHealth <= leaderHealth.startingHealth * healthPercent / 100)
        {
            // Change health in case there is health bar display for enemies
            leaderHealth.currentHealth = leaderHealth.startingHealth * healthPercent / 100;

            // Event occurs if player and leader is alive
            if (player.GetComponent<PlayerHealth>().currentHealth > 0 && !leaderHealth.isDead)
            {

                leader.GetComponent<Collider>().enabled = leaderMoving ? leaderMoving : !leaderMoving;
                leader.GetComponent<Rigidbody>().isKinematic = leaderMoving ? !leaderMoving : leaderMoving;

                // Trigger Event
                LeaderEvent();
            }
        }
    }

    // Start Leader Event
    public void LeaderEvent()
    {
        // Switch to null state
        leader.GetComponent<EnemyStates>().state = EnemyStates.State.NONE;

        // Destroy Aggro Radius and Fire Radius if they are present
        foreach (Transform child in leader.transform)
        {
            if (child.name == "AggroRadius" || child.name == "FireRadius")
            {
                //scripts.Add(child.GetComponent<MonoBehaviour>());
                child.gameObject.SetActive(false) ;
            }
        }

        // Disable Colliders
        ToggleCollider(false);

        // Show Target location
        leaderTarget.SetActive(true);

        // Stop all enemies from moving
        ToggleEnemies(true);

        // Remove Gate
        gate.SetActive(false);

        // Stop leader for dialogue ( if there is any )
        leaderState.StopAgent(true);

        // Move leader to target location 
        Invoke("MoveLeader", leaderStopTime);

        // Prevent player from doing anything 
        playerMove.horizontal = 0;
        playerMove.vertical = 0;
        playerMove.enabled = false;
        playerMelee.enabled = false;
        slowTime.enabled = false;
        weapInven.canFire = false;
        weapInven.enabled = false;

        // Temp disable collider to prevent damage
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        // Set camera target to enemy pos
        playerCamScript.target = leader.transform;
    }

    // End Leader Event
    public void EventOver()
    {
        // Resume all enemy movement
        ToggleEnemies(false);

        // Allow player interactions
        playerMove.enabled = true;
        playerMelee.enabled = true;
        slowTime.enabled = true;
        weapInven.canFire = true;
        weapInven.enabled = true;

        // Remove Gate
        gate.SetActive(true);

        // Re- enabled colliders
        ToggleCollider(true);

        // Re-activate collider for collision
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;

        // Reset target of camera back to player
        playerCamScript.target = player.transform;
    }

    // Remove any enemies that are gone from leader group
    private void RemainingEnemies(List<GameObject> enemyGroup)
    {
        for(int i = 0; i < enemyGroup.Count; i++)
        {
            if(enemyGroup[i] == null)
            {
                enemyGroup.Remove(enemyGroup[i]);
            }
        }
    }

    // Toggle Colliders of leader group
    private void ToggleCollider(bool toggle)
    {
        for(int i = 0; i < leaderGroup.Count; i++)
        {
            if(leaderGroup[i] != leader)
            {
                leaderGroup[i].GetComponent<Collider>().enabled = toggle;
                leaderGroup[i].GetComponent<Rigidbody>().isKinematic = !toggle;
            }
        }
    }

    // Move leader to leaderTarget at patrol speed
    private void MoveLeader()
    {
        leaderMoving = true;

        leaderState.StopAgent(false);

        // Set destination at patrol speed
        leader.GetComponent<EnemyStates>().nav.speed = moveSpeed;
        leader.GetComponent<EnemyStates>().nav.SetDestination(leaderTarget.transform.position);
    }

    // Stop all enemies
    private void ToggleEnemies(bool stop)
    {
        for (int i = 0; i < leaderGroup.Count; i++)
        {
            leaderGroup[i].GetComponent<EnemyStates>().StopAgent(stop);
        }
    }

    private void DisableColliders()
    {
        for (int i = 0; i < leaderGroup.Count; i++)
        {
            leaderGroup[i].GetComponent<Collider>().enabled = false;
            leaderGroup[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void EnableColliders()
    {
        for (int i = 0; i < leaderGroup.Count; i++)
        {
            leaderGroup[i].GetComponent<Collider>().enabled = true;
            leaderGroup[i].GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
