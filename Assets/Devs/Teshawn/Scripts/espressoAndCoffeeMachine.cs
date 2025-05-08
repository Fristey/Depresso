using UnityEngine;

public enum State { Hot, Cold, Coffee }
public class espressoAndCoffeeMachine : MonoBehaviour
{
    [SerializeField] private State mode;
    private bool hasCupForCoffee;

    [SerializeField] private GameObject hotButton;
    [SerializeField] private GameObject coffeeButton;
    [SerializeField] private GameObject coldButton;

    [SerializeField] public Ingredientes ice;
    [SerializeField] public Ingredientes hotWater;
    [SerializeField] public Ingredientes coffee;

    [SerializeField] private MixingCup cup;

    [SerializeField] private Rigidbody handleRb;

    [SerializeField] private HandleScript handle;

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

    public void Dispense(float amount, Ingredientes type)
    {
        Debug.Log("Dispense strength (0–1): " + amount);
        

        currentIngredient(type);
    }

    public void currentIngredient(Ingredientes type)
    {
        if (mode == State.Hot)
        {
            type = hotWater;
        }
        else if (mode == State.Cold)
        {
            type = ice;
        }
        else if (mode == State.Coffee)
        {
            type = coffee;

        }
        handle.currentIngredientes = type;

    }
}
