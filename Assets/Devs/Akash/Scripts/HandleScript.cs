using UnityEngine;

public class HandleScript : MonoBehaviour
{

    private float rotationSpeed = 20f;
    private float minRotation = 50f;
    private float maxRotation = 125f;

    [SerializeField] float gaugeRotationSpeed;
    private float currentAngle = 50f;
    private float gaugeMinRotation = -50;
    private float gaugeMaxRotation = 183;
    private float gaugeCurrentAngle = -50;
    public GameObject gauge;


    [SerializeField] private float minDispenseAmount;

    [SerializeField] private espressoAndCoffeeMachine coffeeMachine;

    [SerializeField] private bool isHeldDown = false;
    private Camera playerCamera;

    [SerializeField] private LookAround lookAround;

    public GameObject objectToDispence;

    [SerializeField] private float maxTimeForDispense;
    [SerializeField] private float timeForDispense;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        coffeeMachine.currentIngredient();
        if (Input.GetMouseButtonDown(0))
        {
            lookAround.canLookAround = false; // Disable looking around when holding the handle
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {

                if (hit.transform == transform)
                {
                    isHeldDown = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lookAround.canLookAround = true; // Enable looking around when not holding the handle
            isHeldDown = false;
        }

        if (isHeldDown)
        {

            float mouseY = Input.GetAxis("Mouse Y");
            currentAngle = Mathf.Clamp(currentAngle + -mouseY * rotationSpeed * Time.deltaTime, minRotation, maxRotation);
            gaugeCurrentAngle = Mathf.Clamp(gaugeCurrentAngle + -mouseY * gaugeRotationSpeed * Time.deltaTime, gaugeMinRotation, gaugeMaxRotation);

            Rotate();

        }
        float rotationAmount = Mathf.InverseLerp(minRotation, maxRotation, currentAngle);

        maxTimeForDispense = Mathf.Lerp(minDispenseAmount, 0, rotationAmount);
        if (maxTimeForDispense < minDispenseAmount)
            timeForDispense += Time.deltaTime;

        if (timeForDispense >= maxTimeForDispense)
        {
            coffeeMachine.Dispense();
            timeForDispense = 0;

        }
    }

    private void Rotate()
    {
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
        gauge.transform.localRotation = Quaternion.Euler(gaugeCurrentAngle, -90f, -90f);
    }
}
