using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0f;  // Movement speed
    public float sprintSpeed = 8f;
    public float rotationSpeed = 3.0f;  // Camera rotation speed
    public float momentumDamping = 5.0f;

    Vector3 moveDirection;
    Vector3 inputVector;
    private float xRotation = 0f;

    private CharacterController characterController;
    public Animator cameraAnimator;

    private float verticalSpeed = 0.0f;
    private float gravity = -9.81f;
    private Transform cameraTransform;

    private bool canMove;
    private bool canRotate;

    private bool sprinting;
    private bool isWalking;

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

        // prevents climbing short things
        //characterController.stepOffset = 0.1f;

        GameEventsManager.instance.playerEvents.onDisablePlayerMovement += FreezePlayer;
        GameEventsManager.instance.playerEvents.onEnablePlayerMovement += UnFreezePlayer;

        // Set this player as the target for all billboards
        Billboard[] billboards = FindObjectsOfType<Billboard>();
        foreach (Billboard b in billboards)
        {
            b.SetTarget(cameraTransform); // or use 'transform' if you want the body
        }
    }

    private void Update()
    {
        GetInput();
        MovePlayer();
        

        cameraAnimator.SetBool("isWalking", isWalking);
    }

    void GetInput(){
        //sprinting = Input.GetButton("Sprint");

        // holding down gives -1, 0, 1
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
        {
            if(canMove){
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                inputVector = new Vector3(horizontalInput, 0, verticalInput);
                inputVector.Normalize();
                inputVector = transform.TransformDirection(inputVector);

                isWalking = true;
                /*
                if (sprinting){
                    moveDirection *= sprintSpeed;
                }
                */
            }
        }
        else{
            // if not then lerp whatever inputVector is at to zero
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
        }

        float adjustedSpeed = moveSpeed;

        // If the player is moving backward (negative Z in local space), halve speed
        if (Vector3.Dot(transform.forward, inputVector) < 0)
        {
            adjustedSpeed *= 0.3f;
        }

        moveDirection = inputVector * adjustedSpeed;

        // calculate the downward force and apply it to player
        verticalSpeed += gravity * Time.deltaTime;
        moveDirection.y = verticalSpeed;

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
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        }
    }

    void MovePlayer(){
        characterController.Move(moveDirection * Time.deltaTime);
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