using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
    }

    public void SetTarget(Transform newTarget){
        target = newTarget;
    }
}