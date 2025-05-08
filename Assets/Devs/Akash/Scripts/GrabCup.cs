using System.Runtime.CompilerServices;
using UnityEngine;

public class GrabCup : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private float grabRange = 3f; // The range of where the held cup is from the camera
    [SerializeField] private float moveForce = 50f;
    [SerializeField] private float throwForce = 10f; // The force applied when throwing the cup
    //[SerializeField] private Transform holdPoint; // The point where the cup will be held
    [SerializeField] private float tiltSpeed = 2f;
    [SerializeField] private float maxTiltAngle = 200f; // The maximum angle of tilt for the cup

    [SerializeField] private float holdDistance = 1.5f; // The distance from the camera to the cup when held
    [SerializeField] private float minScrollDistance = 0.5f; // The minimum distance from the camera to the cup when held 
    [SerializeField] private float maxScrollDistance = 3f; // The maximum distance from the camera to the cup when held
    [SerializeField] private float scrollSpeed = 2f; // The speed of the scroll wheel for adjusting the distance

    private Vector2 tilt; // The tilt of the cup


    private Rigidbody rb; //Reference to the Rigidbody component of the cup    
    private bool isHoldingCup = false; // Flag to check if the cup is being held

    public LookAround lookAround; // Reference to the LookAround script


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grabCup();
        }

        if(Input.GetMouseButtonUp(0))
        {
            DropCup();
        }

        if(isHoldingCup && rb != null)
        {
            MoveCup();
            if(Input.GetMouseButton(1)) // Right mouse button to rotate the cup
            {
                RotateCup();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                lookAround.lockCursor = true; // Enable cursor locking when not rotating the cup
                lookAround.canLookAround = true; // Enable looking around when not holding the cup
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            }

        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            throwCup(); // Throw the cup when pressing the "E" key
            DropCup(); // Drop the cup
        }

/*        holdPoint.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
        holdPoint.rotation = playerCamera.transform.rotation;*/
    }

    private void FixedUpdate()
    {
        if(isHoldingCup && rb != null)
        {
            Vector3 upwards = rb.transform.up; // Get the up direction of the cup
            float upRight = Vector3.Angle(upwards, Vector3.up); // Calculate the angle between the cup's up direction and the world up direction

        }

        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Get the scroll wheel input
        if (scroll != 0f)
        {
            holdDistance = Mathf.Clamp(holdDistance - scroll * scrollSpeed, maxScrollDistance, minScrollDistance); // Adjust the hold distance based on the scroll input
        }
    }

    private void grabCup()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera to the mouse position
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            if (hit.collider.CompareTag("Cup")) // Check if the object hit by the ray has the tag "Cup"
            {
               
                rb = hit.rigidbody; // Get the Rigidbody component of the cup
                rb.useGravity = false; // Disable gravity for the cup while holding it
                rb.linearDamping = 10f; // Increase drag to slow down the cup's movement
                isHoldingCup = true; // Set the flag to indicate that the cup is being held
                rb.angularVelocity = Vector3.zero; // Reset the velocity of the cup
                rb.constraints = RigidbodyConstraints.None; // Remove any constraints on the Rigidbody
            }
        }
    }

    private void DropCup()
    {
        if(rb != null)
        {
            rb.useGravity = true; // Enable gravity for the cup when dropping it
            rb.linearDamping = 0f; // Reset drag to default
            rb.angularDamping = 1f;
            rb.constraints = RigidbodyConstraints.None; // Freeze rotation to prevent unwanted spinning
            rb = null; // Reset the Rigidbody reference
        }
        isHoldingCup = false; // Reset the flag to indicate that the cup is no longer being held
    }

    private void MoveCup()
    {
/*        Vector3 forceDirection = (holdPoint.position - rb.position);*/

        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance; // Calculate the target position for the cup
        Vector3 forceDirection = (targetPosition - rb.position); // Calculate the direction to move the cup towards the hold point
        rb.AddForce(forceDirection * moveForce * Time.deltaTime); // Apply a force to move the cup towards the hold point
    }

    private void RotateCup()
    {
        lookAround.lockCursor = false; // Disable cursor locking while rotating the cup
        lookAround.canLookAround = false; // Disable looking around while holding the cup
        float mouseX = Input.GetAxis("Mouse X"); // Get mouse input for rotation
        float mouseY = Input.GetAxis("Mouse Y");

        tilt.x = Mathf.Clamp(tilt.x + mouseX * tiltSpeed, -maxTiltAngle, maxTiltAngle); // Clamp the tilt angle to prevent over-rotation
        tilt.y = Mathf.Clamp(tilt.y + mouseY * tiltSpeed, -maxTiltAngle, maxTiltAngle); // Clamp the tilt angle to prevent over-rotation    

        Quaternion tiltRotation = Quaternion.Euler(tilt.y, 0f, -tilt.x);
        rb.MoveRotation(playerCamera.transform.rotation * tiltRotation); // Apply the rotation to the cup
    }

    private void throwCup()
    {
        rb.isKinematic = false; // Set the Rigidbody to non-kinematic to allow physics interactions
        rb.transform.parent = null; // Remove the cup from the hold point's hierarchy
        rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse); // Apply a force to throw the cup
        isHoldingCup = false; // Reset the flag to indicate that the cup is no longer being held
    }


}
