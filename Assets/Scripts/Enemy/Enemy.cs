using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float health = 2f;
    public GameObject killEffect;

    public bool isActive;
    public bool isAggro;
    public Transform target;
    public float awarenessRadius = 4f;
    private NavMeshAgent enemyNavMeshAgent;
    private Transform playerTransform;
    private EnemiesInRange enemiesInRange;


    private Vector3 targetPos;
    private Vector3 targetDir;
    public float angle;
    public int lastIndex;

    // Start is called before the first frame update
    void Start()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = PlayerController.Instance.transform;

        if (target == null)
        {
            target = playerTransform;   
        }

        enemiesInRange = EnemiesInRange.Instance;
    }

    // Update is called once per frame
    public void Update()
    {
        // if enemy has no health, the enemy is dead
        if (health <= 0)
        {
            enemiesInRange.RemoveEnemy(this);
            Destroy(gameObject);
        }
        else if (isAggro)
        {
            enemyNavMeshAgent.SetDestination(target.position);
        }

        // if not active then don't do calculation
        if (isActive)
        {
            if (target != null)
            {
                var dist = Vector3.Distance(transform.position, target.position);
                if (dist < awarenessRadius)
                {
                    isAggro = true;
                }

                // construct a targetPos at the player's position but at same y
                targetPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
                targetDir = targetPos - transform.position;

                // get angle from targetDir
                angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                lastIndex = GetIndex(angle);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Instantiate(killEffect, transform.position, Quaternion.identity);
        health -= damage;
    }

    private int GetIndex(float angle)
    {
        //front
        if (angle > -22.5f && angle < 22.6f)
            return 0;
        if (angle >= 22.5f && angle < 67.5f)
            return 7;
        if (angle >= 67.5f && angle < 112.5f)
            return 6;
        if (angle >= 112.5f && angle < 157.5f)
            return 5;


        //back
        if (angle <= -157.5 || angle >= 157.5f)
            return 4;
        if (angle >= -157.4f && angle < -112.5f)
            return 3;
        if (angle >= -112.5f && angle < -67.5f)
            return 2;
        if (angle >= -67.5f && angle <= -22.5f)
            return 1;

        return lastIndex;
    }
}
