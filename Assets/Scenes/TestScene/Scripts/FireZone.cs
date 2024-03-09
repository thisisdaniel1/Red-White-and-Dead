using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireZone : MonoBehaviour
{

    public Transform shotObj;
    public GameObject fireZoneDetection;
    public AudioClip shotSound;
    public AudioSource audioSource;

    public bool shouldShoot = false;
    public Coroutine shootingCoroutine;

    public List<Transform> targetsInRange = new List<Transform>();
    public Transform currentTarget;

    void Start()
    {
    }

    void Update()
    {
        
        if (fireZoneDetection.tag == "active" && !shouldShoot){  
            shouldShoot = true;
            
            shootingCoroutine = StartCoroutine(fireSeq());
        }
        else if (fireZoneDetection.tag == "unactive" && shouldShoot){
            StopShooting();
        }

        UpdateTarget();
    }

    IEnumerator fireSeq(){
        while (shouldShoot){
            FaceTarget();
            // instantiate the shot at the position (and facing) of the attacker but also a little up
            Instantiate(shotObj, new Vector3(transform.position.x, shotObj.position.y, transform.position.z), transform.rotation);

            audioSource.clip = shotSound;
            audioSource.Play();

            // 3 second delay between shots
            yield return new WaitForSeconds(3);
        }
    }

    void StopShooting(){
        StopCoroutine(shootingCoroutine);
        shootingCoroutine = null;

        shouldShoot = false;
        fireZoneDetection.tag = "unactive";
        Debug.Log("Stopped Shooting");
    }

    void UpdateTarget(){
        // if no targets left, skip
        if (targetsInRange.Count == 0){
            currentTarget = null;
            return;
        }

        currentTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // for each target, if that target is closer to this unit, select it
        foreach (Transform potentialTarget in targetsInRange){
            float distance = Vector3.Distance(potentialTarget.position, currentPosition);
            if (distance < closestDistance){
                closestDistance = distance;
                currentTarget = potentialTarget;
            }
        }
    }

    public void AddTarget(Transform target){
        // if this enemy was not already in range, add it to the list
        if (!targetsInRange.Contains(target)){
            targetsInRange.Add(target);
            UpdateTarget();
        }
    }

    public void RemoveTarget(Transform target){
        // if this enemy exits out of range, remove it from the list
        if (targetsInRange.Contains(target)){
            targetsInRange.Remove(target);
            UpdateTarget();
        }
    }

    void FaceTarget(){
        if (currentTarget == null){
            return;
        }

        Vector3 directionToTarget = currentTarget.position - transform.position;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1000f);
    }
}
