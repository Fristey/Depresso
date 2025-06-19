using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> ingredientesNames;
    public GameObject normalCup;
    public Recipes drinkToserve;

    OrderManager orderManager;

    public float maxAmount = 100f;
    public float currentAmount = 0f;

    private void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
    }

    private void Update()
    {
        CreateDrink();
    }

    /// <summary>
    /// Checks if the Ingredients list matches the recipe Ingredients list 
    /// then empties the cup list and turns it into a drink(Coffee cup like cosmos coffee ect)
    /// </summary>
    /// <returns>de drink you made</returns>
    public bool CreateDrink()
    {
        ingredientesNames.Sort();
        cupIngredientes.Sort();
        for (int i = 0; i < orderManager.possibleDrinks.Count; i++)
        {
            orderManager.possibleDrinks[i].requiredIngredientes.Sort();
            if (cupIngredientes.SequenceEqual(orderManager.possibleDrinks[i].requiredIngredientes))
            {
                drinkToserve = orderManager.possibleDrinks[i];
                //Elger: change the viuals for the cup to recipe drink(the new drink like Cosmos coffee
                ingredientesNames.Clear();
                cupIngredientes.Clear();
                return orderManager.possibleDrinks[i];
            }
        }

        return false;
    }


    public void Spill(float amount)
    {
        //Debug.Log("Spilling amount: " + amount);
        currentAmount = Mathf.Max(currentAmount - amount, 0f);
        //Debug.Log("Current Amount:" + currentAmount);
    }
}
