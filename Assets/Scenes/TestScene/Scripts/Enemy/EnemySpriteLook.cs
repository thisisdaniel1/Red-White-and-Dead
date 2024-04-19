using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteLook : MonoBehaviour
{
    public Transform target;
    public bool canLookVertically;

    public void SetTarget(Transform newTarget){
        target = newTarget;
    }

    void Update(){
        if (canLookVertically){
            transform.LookAt(target);
        }
        else{
            Vector3 modifiedTarget = target.position;
            modifiedTarget.y = transform.position.y;

            transform.LookAt(modifiedTarget);
        }
    }
}
