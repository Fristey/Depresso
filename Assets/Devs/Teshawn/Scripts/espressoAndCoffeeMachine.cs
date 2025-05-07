using System.Collections.Generic;
using UnityEngine;

public class espressoAndCoffeeMachine : MonoBehaviour
{
    private Ingredientes ingredient; //need the scriptable object that shows what it is(the scriptableObject)

    public List<Ingredientes> ingredientesInMachine;

    public GameObject drinkToDispence;

    public void CoffeeMixes()
    {

    }

    private void Coffie(GameObject drink)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Ingredientes itemToMix = collision.gameObject.GetComponent<Ingredientes>();
        if (collision.gameObject.GetComponent<Ingredientes>() != null)
        {
            ingredientesInMachine.Add(itemToMix);
        }
    }
}
