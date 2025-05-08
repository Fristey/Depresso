using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // Reference to the CharacterController component
    private float speed = 5f; // Movement speed of the player
    private float gravity = -9.81f; // Gravity value

    Vector3 velocity; // Velocity vector for gravity

    void Update()
    {
        // Call the Movement method every frame
        Movement();
    }


    private void Movement()
    {
/*        if (!GameManager.Instance.IsEnded)
        {*/

            // Get input from the player
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            // Calculate movement direction
            Vector3 movement = transform.right * xMove + transform.forward * zMove;
            // Move the player
            controller.Move(movement * speed * Time.deltaTime);

            // Apply gravity if the player is not grounded
            if (!controller.isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }

            // Apply the velocity to the player
            controller.Move(velocity * Time.deltaTime);
        }
    //}
}
