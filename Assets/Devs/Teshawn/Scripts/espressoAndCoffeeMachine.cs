using System.Collections.Generic;
using UnityEngine;

public class espressoAndCoffeeMachine : MonoBehaviour
{
    public List<Ingredientes> ingredientesInMachine;
    [SerializeField] private Recipes recipieMade;

    private Transform DrinkSpawnPoint;
    public void CoffeeMixes()
    {
        if (ingredientesInMachine.Equals(recipieMade.requiredIngredientes))
        {
            Instantiate(recipieMade.drink, DrinkSpawnPoint.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            ingredientesInMachine.AddRange(collision.gameObject.GetComponent<MixingCup>().cupIngredientes);
            collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Clear();
        }
    }
}
