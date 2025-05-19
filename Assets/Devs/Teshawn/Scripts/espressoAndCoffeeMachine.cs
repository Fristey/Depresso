using UnityEngine;

public enum State { Hot, Cold, Coffee }
public class espressoAndCoffeeMachine : MonoBehaviour
{
    public enum FixedOrBroken { Fixed, Broken }
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
            else
            {
                cup = null;
            }
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
