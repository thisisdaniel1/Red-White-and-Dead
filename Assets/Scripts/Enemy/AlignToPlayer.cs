using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;

    public float angle;
    public int lastIndex;

    public void SetTarget(Transform newTarget){
        player = newTarget;
    }

    void Update(){
        // construct a targetPos at the player's position but at same y
        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        targetDir = targetPos - transform.position;

        // get angle from targetDir
        angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        lastIndex = GetIndex(angle);
    }

    private int GetIndex(float angle){
        // front
        if (angle > -22.5f && angle < 22.6f){
            return 0;
        }

        // back
        if (angle <= -157.5 || angle >= 157.5f){
            return 4;
        }

        return lastIndex;
    }
}
