using UnityEngine;

public class ShotTraj : MonoBehaviour
{
    public Vector3 shotTraj;
    public float shotLifetime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // the speed of the shot
        shotTraj = transform.forward * 5;
        GetComponent<Rigidbody>().velocity = shotTraj;

        Destroy(gameObject, shotLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other){
        Debug.Log("shot in contact");
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            Debug.Log("shot hit enemy");
            Destroy(gameObject);
        }
    }
}
