using UnityEngine;

public class FireZoneDetection : MonoBehaviour
{
    public FireZone fireZone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            fireZone.AddTarget(other.transform);
            gameObject.tag = "active";
        }
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            fireZone.RemoveTarget(other.transform);
            gameObject.tag = "unactive";
        }
    }
}
