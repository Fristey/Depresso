using UnityEngine;

public enum State { Hot, Cold, Coffee }
public class espressoAndCoffeeMachine : MonoBehaviour
{
    [SerializeField] private MixingCup cup;
    [SerializeField] private Rigidbody handleRb;
    [SerializeField] private HandleScript handle;

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
