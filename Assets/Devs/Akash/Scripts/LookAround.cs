using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Transform playerBody; // Reference to the player's body transform

    [SerializeField] private float mouseSensitivity = 100f; // Sensitivity of the mouse movement
    private float xRotation = 0f; // Rotation around the X-axis
    public bool canLookAround = true; // Flag to enable/disable looking around
    public bool lockCursor = true;

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        if (lockCursor)
        {
            LockCursor();
        }  
    }

    void Update()
    {
        if (!lockCursor)
        {
            //UnlockCursor();
            LockCursor();
        }
        else
        {
            LockCursor();
        }
            MoveHeadAround();
    }



    private void MoveHeadAround()
    {
        if (canLookAround)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Adjust the X rotation based on mouse Y input
            xRotation -= mouseY;
            // Clamp the X rotation to prevent over-rotation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Apply the rotation to the camera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // Rotate the player's body based on mouse X input
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
