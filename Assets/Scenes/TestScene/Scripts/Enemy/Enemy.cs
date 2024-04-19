using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float health = 2f;
    public EnemyManager enemyManager;
    public GameObject killEffect;

    public bool isActive;
    public bool isAggro;
    public Material aggroMat;
    public Transform target;
    public float awarenessRadius = 4f;
    private NavMeshAgent enemyNavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = EnemyManager.Instance;
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy has no health, the enemy is dead
        if (health <= 0){
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }

        // if not active then don't do calculation
        if (isActive){
            EnemyManager.Instance.AddEnemy(this);
            if (target != null){
                var dist = Vector3.Distance(transform.position, target.position);
                if (dist < awarenessRadius){
                    isAggro = true;
                }
            }
        }

        if (isAggro){
            GetComponent<MeshRenderer>().material = aggroMat;
            enemyNavMeshAgent.SetDestination(target.position);
        }
    }

    public void TakeDamage(float damage){
        Instantiate(killEffect, transform.position, Quaternion.identity);
        health -= damage;
    }

    public void SetTarget(Transform newTarget){
        target = newTarget;
        GetComponentInChildren<EnemySpriteLook>().SetTarget(newTarget);
        GetComponent<AlignToPlayer>().SetTarget(newTarget);
    }
}
