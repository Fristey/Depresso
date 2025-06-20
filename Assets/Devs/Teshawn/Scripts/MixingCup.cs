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
                visualSwapper.Swap(drinkToserve.drink, drinkToserve.position);
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
