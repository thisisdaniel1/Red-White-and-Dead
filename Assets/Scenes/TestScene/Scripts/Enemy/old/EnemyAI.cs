using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    private EnemyAwareness enemyAwareness;
    private Transform playersTransform;
    private NavMeshAgent enemyNavMeshAgent;

    void Start(){
        enemyAwareness = GetComponent<EnemyAwareness>();
        FindLocalPlayer();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        if (enemyAwareness.isAggro){
            enemyNavMeshAgent.SetDestination(playersTransform.position);
        }
        else{
            enemyNavMeshAgent.SetDestination(transform.position);
        }
    }

    void FindLocalPlayer(){
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach(PlayerController player in players){
            Debug.Log("scanning");
            if(player.GetComponent<PhotonView>().IsMine){
                playersTransform = player.transform;
                Debug.Log("found");
                break;
            }
        }
    }
}
