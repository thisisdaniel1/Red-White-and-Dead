using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public bool isAggro;
    public Material aggroMat;
    private Transform playersTransform;
    public float awarenessRadius = 4f;

    void Start(){
    }

    void Update(){
        var dist = Vector3.Distance(transform.position, playersTransform.position);

        if (dist < awarenessRadius){
            isAggro = true;
        }

        if (isAggro){
            GetComponent<MeshRenderer>().material = aggroMat;
        }
    }
}
