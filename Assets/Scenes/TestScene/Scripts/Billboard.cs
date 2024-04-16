using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Billboard : MonoBehaviour
{
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        BillboardManager.Instance.AddBillboard(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
    }

    public void SetTarget(Transform newTarget){
        target = newTarget;
    }
}
