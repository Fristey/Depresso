using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GrabCup : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float grabRange = 3f;
    [SerializeField] private float moveForce = 5000f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float tiltSpeed = 2f;
    [SerializeField] private float maxTiltAngle = 200f;

    [SerializeField] private float holdDistance = 1.5f;
    [SerializeField] private float minScrollDistance = 0.5f;
    [SerializeField] private float maxScrollDistance = 3f;
    [SerializeField] private float scrollSpeed = 70f;

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject vfxExtinguisher;

    [SerializeField] private LayerMask pickupLayer;

    private Vector2 tilt;
    private Vector3 holdPointPosition;

    private Quaternion relativeRotation = Quaternion.identity;


    private Rigidbody rb;
    private bool isHoldingCup = false;
    private bool isRotating = false;

    public LookAround lookAround;

    private espressoAndCoffeeMachine machine;

    //Added by Elger
    private YarnBall curBallScript;


    //Added by Teshawn
    private CamSwapManager swapManager;

    private void Start()
    {
        machine = FindFirstObjectByType<espressoAndCoffeeMachine>();
        swapManager = GetComponent<CamSwapManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grabCup();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropCup();

            isRotating = false;
            Cursor.lockState = CursorLockMode.None;
            lookAround.lockCursor = true;
            lookAround.canLookAround = true;
        }

        if (isHoldingCup && rb != null)
        {
            MoveCup();

            if (Input.GetMouseButtonDown(1)) // Right mouse button to rotate the cup
            {
                isRotating = true;
                relativeRotation = Quaternion.Inverse(playerCamera.transform.rotation) * rb.rotation;
                Cursor.lockState = CursorLockMode.Locked;

                lookAround.lockCursor = false;
                lookAround.canLookAround = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isRotating = false;
                Cursor.lockState = CursorLockMode.None;
                lookAround.lockCursor = true;
                lookAround.canLookAround = true;
            }

            if (isRotating) // Rotate the cup while holding the right mouse button
            {
                RotateCup();
            }
            else
            {
                rb.MoveRotation(playerCamera.transform.rotation * relativeRotation); // Keep the cup aligned with the camera rotation when not rotating it
            }

            if (rb.CompareTag("Extinguisher") && particleSystem != null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (!particleSystem.isPlaying)
                    {
                        particleSystem.Play();
                        vfxExtinguisher.SetActive(true);
                    }
                }
                else
                {
                    if (particleSystem.isPlaying)
                    {
                        particleSystem.Stop();
                        vfxExtinguisher.SetActive(false);
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                throwCup();
                DropCup();
            }

            /*        holdPoint.position = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
                    holdPoint.rotation = playerCamera.transform.rotation;*/
        }
    }

    private void FixedUpdate()
    {

        if (isHoldingCup && rb != null && Input.GetMouseButtonDown(1))
        {
            Vector3 upwards = rb.transform.up;
            float upRight = Vector3.Angle(upwards, Vector3.up);

            if (upRight < 30f)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(upwards, Vector3.up);
                Vector3 balance = new Vector3(targetRotation.x, targetRotation.y, targetRotation.z) * 10f;
                rb.AddTorque(balance);
            }


        }

        if (rb != null)
        {
            float tiltAngle = Vector3.Angle(Vector3.up, rb.transform.up);

            if (tiltAngle > 30f)
            {
                float spillRate = (tiltAngle - 50f) * 0.1f; // Calculate the spill rate based on the angle
                MixingCup mixingCup = rb.GetComponent<MixingCup>();
                if (mixingCup != null)
                {
                    mixingCup.Spill(spillRate * Time.deltaTime);
                }
            }
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            //holdDistance = Mathf.Clamp(holdDistance - scroll * scrollSpeed, maxScrollDistance, minScrollDistance); // Adjust the hold distance based on the scroll input
            holdDistance = Mathf.Lerp(holdDistance, Mathf.Clamp(holdDistance - scroll * scrollSpeed, maxScrollDistance, minScrollDistance), Time.deltaTime * 5f);
        }

        Debug.Log("fixed update running");
    }

    private void grabCup()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grabRange, pickupLayer))
        {
            rb = hit.rigidbody;
            holdPointPosition = rb.transform.InverseTransformPoint(hit.point);

            relativeRotation = Quaternion.Inverse(playerCamera.transform.rotation) * rb.rotation;
            rb.useGravity = false;
            rb.linearDamping = 10f;
            isHoldingCup = true;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
            if (hit.collider.gameObject.CompareTag("Untagged") || hit.collider.gameObject.CompareTag("Extinguisher"))
            {

                rb = hit.rigidbody;
                holdPointPosition = rb.transform.InverseTransformPoint(hit.point);

                relativeRotation = Quaternion.Inverse(playerCamera.transform.rotation) * rb.rotation;
                rb.useGravity = false;
                rb.linearDamping = 10f;
                isHoldingCup = true;
                rb.angularVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.None;
            }
            else if (hit.collider.CompareTag("Coffee"))
            {
                machine.mode = State.Coffee;
            }
            else if (hit.collider.CompareTag("Ice"))
            {
                machine.mode = State.Cold;
            }
            else if (hit.collider.CompareTag("HotWater"))
            {
                machine.mode = State.Hot;
            }
            else if (hit.collider.CompareTag("Tablet"))
            {
                GetComponent<CamSwapManager>().isLookingAtTablet = true;
            }
            else if (hit.collider.CompareTag("YarnSpawner"))
            {
                YarnSpawner curSpawn = hit.collider.gameObject.GetComponent<YarnSpawner>();

                if (curSpawn.GrabYarn())
                {
                    curBallScript = curSpawn.yarnScript;

                    rb = curSpawn.yarnRb;
                    rb.useGravity = false;
                    rb.linearDamping = 10f;
                    isHoldingCup = true;
                    rb.angularVelocity = Vector3.zero;
                    rb.constraints = RigidbodyConstraints.None;
                }
            }else if (hit.collider.CompareTag("Book"))
            {
                swapManager.isLookingAtBook = true;
            }
        }
    }

    private void DropCup()
    {
        if (rb != null)
        {
            rb.useGravity = true;
            rb.linearDamping = 0f;
            rb.angularVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.angularDamping = 1f;
            rb.constraints = RigidbodyConstraints.None;
            rb = null;

            YarnDrop();
        }
        holdDistance = 1.5f;
        isHoldingCup = false;
    }

    private void MoveCup()
    {
        /*        Vector3 forceDirection = (holdPoint.position - rb.position);*/

        Vector3 targetGrabPoint = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        Vector3 targetPosition = targetGrabPoint - rb.transform.TransformVector(holdPointPosition);
        Vector3 forceDirection = (targetPosition - rb.position);
        rb.AddForce(forceDirection * moveForce * Time.deltaTime);

        if (!Input.GetMouseButton(1))
        {
            rb.MoveRotation(playerCamera.transform.rotation * relativeRotation); // Keep the cup aligned with the camera rotation when not rotating it
        }
    }

    private void RotateCup()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        /*        tilt.x = Mathf.Clamp(tilt.x + mouseX * tiltSpeed, -maxTiltAngle, maxTiltAngle); // Clamp the tilt angle to prevent over-rotation
                tilt.y = Mathf.Clamp(tilt.y + mouseY * tiltSpeed, -maxTiltAngle, maxTiltAngle); // Clamp the tilt angle to prevent over-rotation    */

        Vector3 cameraRight = playerCamera.transform.right;
        Vector3 cameraUp = playerCamera.transform.up;

        Quaternion horizontal = Quaternion.AngleAxis(mouseX * tiltSpeed, Vector3.up);
        Quaternion vertical = Quaternion.AngleAxis(mouseY * tiltSpeed, Vector3.right);

        //Quaternion tiltRotation = Quaternion.Euler(tilt.y, 0f, -tilt.x);

        relativeRotation = horizontal * vertical * relativeRotation;

        rb.MoveRotation(playerCamera.transform.rotation * relativeRotation);
    }

    private void throwCup()
    {
        rb.isKinematic = false;
        rb.transform.parent = null;
        rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        isHoldingCup = false;

        YarnDrop();
    }

    private void YarnDrop()
    {
        if(curBallScript != null)
        {
            curBallScript.StartDistraction();

            curBallScript = null;
        }
    }
}
