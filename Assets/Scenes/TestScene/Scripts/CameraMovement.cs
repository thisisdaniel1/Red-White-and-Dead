using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float defaultSpeed = 7f;
    public float defaultZoomSpeed = 2500f;

    float speed;
    float zoomSpeed;
    float rotationSpeed = 30f;

    public float maxHeight = 5f;
    public float minHeight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)){
            speed = 20f;
            zoomSpeed = 5000f;
        }
        else{
            speed = defaultSpeed;
            zoomSpeed = defaultZoomSpeed;
        }


        float horizontalSpeed = transform.position.y * speed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalSpeed = transform.position.y * speed * Input.GetAxis("Vertical") * Time.deltaTime;
        float scrollSpeed = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        // prevents scrolling by checking when trying to scroll past max or min height
        // remember if scrollSpeed is 0 then the camera's forward move is also 0
        if ((transform.position.y >= maxHeight) && (scrollSpeed > 0)){
            scrollSpeed = 0;
        }
        else if ((transform.position.y <= minHeight) && (scrollSpeed < 0)){
            scrollSpeed = 0;
        }

        // prevents "tunneling" if the camera is moving to a point beneath or above max or min height
        if ((transform.position.y + scrollSpeed) > maxHeight){
            scrollSpeed = maxHeight - transform.position.y;
        }
        else if ((transform.position.y + scrollSpeed) < minHeight){
            scrollSpeed = minHeight - transform.position.y;
        }

        if (float.IsNaN(scrollSpeed)){
            scrollSpeed = 0;
        }

        // Vertical movement vector for camera zooming
        Vector3 verticalMove = new Vector3(0, scrollSpeed, 0);

        // we don't want the camera to move left and right, 
        // we want it to move sideways based on direction of camera
        Vector3 lateralMove = horizontalSpeed * transform.right;

        // uses vector projection where with a vector, the main path and a shadow is drawn
        // we are only interested in the shadow which we will extend
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= verticalSpeed;

        Vector3 move = verticalMove + lateralMove + forwardMove;

        transform.position += move;

        // Check if mouse is at the edge of the screen
        if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width ||
            Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height)
        {
            // Rotate the camera based on mouse position
            float mouseX = Input.mousePosition.x / Screen.width;
            // float mouseY = Input.mousePosition.y / Screen.height;

            float rotationSpeedX = mouseX * rotationSpeed * Time.deltaTime;
            // float rotationSpeedY = mouseY * rotationSpeed * Time.deltaTime;

            /* Clamp vertical rotation to prevent flipping
            float currentRotationX = transform.eulerAngles.x;
            float newRotationX = Mathf.Clamp(currentRotationX - rotationSpeedY, -80f, 80f);
            */

            // Rotate around the world up vector for x-axis rotation
            transform.Rotate(Vector3.up, rotationSpeedX, Space.World);

            // Rotate around the local right vector for y-axis rotation
            // transform.rotation = Quaternion.Euler(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
