using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{

    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player presses right click
        if (Input.GetMouseButtonDown(1)){
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // if the ray hit the ground mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)){
                agent.SetDestination(hit.point);
            }
        }
    }
}
