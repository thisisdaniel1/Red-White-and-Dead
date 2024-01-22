using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    public Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    public PanelManager panelManager;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // clears prompt text if not looking at one
        playerUI.UpdateText(string.Empty);

        // a new ray from where the player is looking
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        // returns info to hitInfo, only runs if ray hit something
        if(Physics.Raycast(ray, out RaycastHit hitInfo, distance, mask)){
            if (hitInfo.collider.GetComponent<Interactable>() != null){
                // get the interactable the raycast hit
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                // set the text onscreen to the text on the interactable script
                playerUI.UpdateText(interactable.promptMessage);
                
                // if player presses the interact key
                if (Input.GetKeyDown(KeyCode.E)){
                    interactable.BaseInteract(panelManager);
                }
            }
        }
    }
}
