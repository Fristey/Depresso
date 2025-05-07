using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class espressoAndCoffeeMachine : MonoBehaviour
{
    public List<Ingredientes> ingredientesInMachine;
    [SerializeField] private Rigidbody handleRb;

    public List<Recipes> recipes;
    public Recipes currentRecipe;

    [SerializeField] private Transform DrinkSpawnPoint;

    private void Update()
    {
        CheckCurrentRecipe();
        DrinkSpawn();
    }
    public void DrinkSpawn()
    {
        if (currentRecipe != null)
        {
            Instantiate(currentRecipe.drink, DrinkSpawnPoint.position, Quaternion.identity);
            ingredientesInMachine.Clear();
        }
    }

    public void CheckCurrentRecipe()
    {
        if (ingredientesInMachine.Count > 0)
        {
            foreach (var recipeInTheMaking in recipes)
            {
                Debug.Log("recipeInTheMaking");
                if (ingredientesInMachine.SequenceEqual(recipeInTheMaking.requiredIngredientes))
                {
                    Debug.Log("checking");
                    currentRecipe = recipeInTheMaking;
                }

            }
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

    public void Dispense(float amount)
    {
        Debug.Log("Dispense strength (0–1): " + amount);
    }
}
