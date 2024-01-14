using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public int damage;

    public Camera camera;

    public float fireRate;

    private float nextFire;

    // Update is called once per frame
    void Update()
    {

        if (nextFire > 0){
            nextFire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextFire <= 0){
            nextFire = 1 / fireRate;
        }
    }

    void Fire(){
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
    }
}
