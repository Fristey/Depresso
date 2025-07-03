using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//ask for a threshHold for turning thr cup empty
// make sure to add an int for the epmty threshold if needed
public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> ingredientesNames;
    public GameObject normalCup;
    public string drinkName;
    public Recipes drinkToserve;

    OrderManager orderManager;

    public float maxAmount = 100f;
    public float currentAmount = 0f;

    [SerializeField] private VisualSwapper visualSwapper;

    private void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
    }

    private void Update()
    {
        CreateDrink();

        if (currentAmount <= 0)
        {
            visualSwapper.ResetVisual();
            cupIngredientes.Clear();
            ingredientesNames.Clear();
            drinkToserve = null;
        }

        if(currentAmount > maxAmount)
        {
            currentAmount = maxAmount;
        }
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
                drinkName = orderManager.possibleDrinks[i].nameOfDrink;
                visualSwapper.Swap(drinkToserve.drink, drinkToserve.position);
                currentAmount += 20;
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
        currentAmount = Mathf.Round(currentAmount);
        //Debug.Log("Current Amount:" + currentAmount);
    }
}
