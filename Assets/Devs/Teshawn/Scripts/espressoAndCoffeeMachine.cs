using UnityEngine;

public enum State { Hot, Cold, Coffee }
public class espressoAndCoffeeMachine : MonoBehaviour
{
    [SerializeField] private float maxFixingTime = 10;
    [SerializeField] private float fixingTime;
    [SerializeField] private bool isfixing;

    public enum FixedOrBroken { Fixed, Broken }
    public GameObject VFX;
    [SerializeField] private MixingCup cup;
    [SerializeField] private Rigidbody handleRb;
    [SerializeField] private HandleScript handle;
    [SerializeField] private Transform dispensePoint;

    public FixedOrBroken fixedOrBroken;
    public State mode;

    public Ingredientes ice;
    public Ingredientes hotWater;
    public Ingredientes coffee;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            if (cup == null)
            {
                cup = collision.gameObject.GetComponent<MixingCup>();
            }
        }
        else if (collision.gameObject.CompareTag("Wrench"))
        {
            isfixing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isfixing) 
        {
            isfixing = false; 
        }

        if (cup != null) 
        {
            cup = null;
        }
    }


    private void Update()
    {
        if (fixedOrBroken == FixedOrBroken.Broken)
        {
            VFX.SetActive(true);
            if (isfixing)
            {
                fixingTime += Time.deltaTime;
            }
            else
            {
                fixingTime -= Time.deltaTime;
            }

            if (fixingTime < 0)
            {
                fixingTime = 0;
            }

            if (fixingTime > maxFixingTime)
            {
                fixedOrBroken = FixedOrBroken.Fixed;
                fixingTime = 0;
            }
        }
        else
        {
            VFX.SetActive(false);
        }
    }

    public void Dispense()
    {
        if (fixedOrBroken == FixedOrBroken.Fixed)
        {
            currentIngredient();
            Instantiate(handle.objectToDispence, dispensePoint.position, Quaternion.identity);
        }
    }

    public void currentIngredient()
    {

        if (mode == State.Hot)
        {
            handle.objectToDispence = hotWater.ingredientToSpawn;
        }
        else if (mode == State.Cold)
        {
            handle.objectToDispence = ice.ingredientToSpawn;
        }
        else if (mode == State.Coffee)
        {
            handle.objectToDispence = coffee.ingredientToSpawn;
        }
    }
}
