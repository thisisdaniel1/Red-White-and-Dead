using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDoor : MonoBehaviour
{
    // on the moving door, create an idle door and moving door animation

    // remember to add a box collider set as trigger on the DoorBase object

    // drag the actual moving door child into slot in inspector
    public Animator doorAnim;

    public GameObject areaToSpawn;

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            doorAnim.SetTrigger("OpenDoor");
        }

        // turn off loop time on OpenDoor animation
    }
}