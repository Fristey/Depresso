using UnityEngine;

public class HandleScript : MonoBehaviour
{

    private float rotationSpeed = 20f;
    private float minRotation = 50f;
    private float maxRotation = 125f;

    [SerializeField] private espressoAndCoffeeMachine coffeeMachine;

    [SerializeField] private bool isHeldDown = false;
    private Camera playerCamera;

    [SerializeField] private LookAround lookAround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lookAround.canLookAround = false; // Disable looking around when holding the handle
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                
                if(hit.transform == transform)
                {
                    Debug.Log("handle");
                    isHeldDown = true;
                }
            }

        }

        if(Input.GetMouseButtonUp(0))
        {
            lookAround.canLookAround = true; // Enable looking around when not holding the handle
            isHeldDown = false;
        }

        if (isHeldDown)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float currentX = transform.localEulerAngles.x;
            if(currentX > 180)
            {
                currentX -= 360;
            }

            float newAngle = Mathf.Clamp(currentX + mouseY * rotationSpeed * Time.deltaTime, minRotation, maxRotation);

            transform.localEulerAngles = new Vector3(newAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);

            float normalized = Mathf.InverseLerp(minRotation, maxRotation, newAngle);
            coffeeMachine.Dispense(normalized);
        }
    }


}
