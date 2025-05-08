using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum State { Hot, Cold, Coffee }
public class espressoAndCoffeeMachine : MonoBehaviour
{
    private State mode;
    private bool hasCupForCoffee;

    [SerializeField] private GameObject hotButton;
    [SerializeField] private GameObject coffeeButton;
    [SerializeField] private GameObject coldButton;

    [SerializeField] private Ingredientes ice;
    [SerializeField] private Ingredientes hotWater;
    [SerializeField] private Ingredientes coffee;

    [SerializeField] private MixingCup cup;

    [SerializeField] private Rigidbody handleRb;
    [SerializeField] private Transform DrinkSpawnPoint;



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
    }
}
