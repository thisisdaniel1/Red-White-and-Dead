using UnityEngine;

public class UnitClick : MonoBehaviour
{

    public Camera cam;
    public GameObject groundMarker;

    public LayerMask clickable;
    public LayerMask ground;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // if we hit a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable)){

                // holding shift clicked, uses GetKey for hold functionality
                if (Input.GetKey(KeyCode.LeftShift)){
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }

                // normal clicked
                else{
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            // if we don't and not shift clicking
            else{

                if (!Input.GetKey(KeyCode.LeftShift)){
                    UnitSelections.Instance.DeselectAll();
                }

            }
        }
        if (Input.GetMouseButtonDown(1)){
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)){
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
    }
}
