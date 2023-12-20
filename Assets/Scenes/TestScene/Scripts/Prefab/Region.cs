using UnityEngine;

public class Region : MonoBehaviour
{
    public string name;
    public bool isOccupied = false;
    public GameObject occupyingUnit;

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Unit")){
            isOccupied = true;
            occupyingUnit = null;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Unit")){
            isOccupied = false;
            occupyingUnit = null;
        }
    }
}
