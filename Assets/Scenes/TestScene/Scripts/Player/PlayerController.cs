using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.5f;  // Movement speed
    public float rotationSpeed = 3.0f;  // Camera rotation speed

    private CharacterController characterController;

    private float verticalSpeed = 0.0f;
    private float gravity = -9.81f;
    private Transform cameraTransform;

    private bool canMove;
    private bool canRotate;

    public static PlayerController Instance;

    void Awake(){
        Instance = this;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = transform.Find("Camera"); // Change "Camera" to the actual name of your camera child
        canMove = true;
        canRotate = true;
        FreezeCursor();

        // prevents climbing short things, like the sleeper pods
        characterController.stepOffset = 0.1f;
    }

    private void Update()
    {
        if(canMove){
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            // calculate the downward force and apply it to player
            verticalSpeed += gravity * Time.deltaTime;
            moveDirection.y = verticalSpeed;

            characterController.Move(moveDirection * Time.deltaTime);
        }

        // reset gravitational force
        if(characterController.isGrounded){
            verticalSpeed = 0;
        }

        if(canRotate){
            // Player camera rotation
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Rotate the player object left and right
            transform.Rotate(Vector3.up * mouseX);

            // Rotate the camera up and down
            cameraTransform.Rotate(Vector3.left * mouseY);
        }
    }

    public void FreezePlayer(){
        canMove = false;
        canRotate = false;
    }

    public void UnFreezePlayer(){
        canMove = true;
        canRotate = true;
    }

    // default cursor
    public void FreezeCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnFreezeCursor(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
