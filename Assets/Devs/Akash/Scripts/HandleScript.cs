using UnityEngine;

public class HandleScript : MonoBehaviour
{

    private float rotationSpeed = 20f;
    private float minRotation = 50f;
    private float maxRotation = 125f;
    private float currentAngle = 50f;

    [SerializeField] private espressoAndCoffeeMachine coffeeMachine;

    public Ingredientes currentIngredientes;

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
        coffeeMachine.currentIngredient(currentIngredientes);
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
            currentAngle = Mathf.Clamp(currentAngle + -mouseY * rotationSpeed * Time.deltaTime, minRotation, maxRotation);

            Rotate();

            float rotationAmount = Mathf.InverseLerp(minRotation, maxRotation, currentAngle);
            coffeeMachine.Dispense(rotationAmount, currentIngredientes);
        }
    }

    private void Rotate()
    {
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }

    
}
