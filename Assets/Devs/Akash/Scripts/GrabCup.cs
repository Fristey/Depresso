using UnityEngine;

public class GrabCup : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private float grabRange = 3f; // The range of where the held cup is from the camera
    [SerializeField] private float moveForce = 50f;
    [SerializeField] private float throwForce = 10f; // The force applied when throwing the cup
    [SerializeField] private Transform holdPoint; // The point where the cup will be held

    private Rigidbody rb; //Reference to the Rigidbody component of the cup    
    private bool isHoldingCup = false; // Flag to check if the cup is being held


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
            }
            
        }
    }

    private void grabCup()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera to the mouse position
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            Debug.Log(hit.transform.name); // Log the name of the object hit by the ray
            if (hit.collider.CompareTag("Cup")) // Check if the object hit by the ray has the tag "Cup"
            {
                Debug.LogWarning("Cup grabbed!"); // Log a warning message when the cup is grabbed
                rb = hit.rigidbody; // Get the Rigidbody component of the cup
                rb.useGravity = false; // Disable gravity for the cup while holding it
                rb.linearDamping = 10f; // Increase drag to slow down the cup's movement
                isHoldingCup = true; // Set the flag to indicate that the cup is being held
            }
        }
    }

    private void DropCup()
    {
        Debug.Log("Cup dropped!"); // Log a message when the cup is dropped
        if(rb != null)
        {
            rb.useGravity = true; // Enable gravity for the cup when dropping it
            rb.linearDamping = 0f; // Reset drag to default
            rb = null; // Reset the Rigidbody reference
        }
        isHoldingCup = false; // Reset the flag to indicate that the cup is no longer being held
    }

    private void MoveCup()
    {
        Vector3 forceDirection = (holdPoint.position - rb.position);
        rb.AddForce(forceDirection * moveForce * Time.deltaTime); // Apply a force to move the cup towards the hold point
    }

    private void RotateCup()
    {
        float mouseX = Input.GetAxis("Mouse X"); // Get mouse input for rotation
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(-mouseY, mouseX, 0f);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation)); // Rotate the cup based on mouse input
    }

    private void throwCup()
    {
        rb.isKinematic = false; // Set the Rigidbody to non-kinematic to allow physics interactions
        rb.transform.parent = null; // Remove the cup from the hold point's hierarchy
        rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse); // Apply a force to throw the cup
        isHoldingCup = false; // Reset the flag to indicate that the cup is no longer being held
    }


}
