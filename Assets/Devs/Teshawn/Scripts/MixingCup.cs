using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MixingCup : MonoBehaviour
{
    public List<Ingredientes> cupIngredientes;
    public List<string> taglist = new List<string> { "Coffee", "Ice", "Milk" };
    public List<string> ingredientesNames;
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
    public bool CreateDrink()
    {
        ingredientesNames.Sort();
        for (int i = 0; i < orderManager.possibleDrinks.Count; i++)
        {
            orderManager.possibleDrinks[i].requiredIngredientes.Sort();

            if (cupIngredientes.SequenceEqual(orderManager.possibleDrinks[i].requiredIngredientes))
            {
                drinkToserve = orderManager.possibleDrinks[i];
                ingredientesNames.Clear();
                cupIngredientes.Clear();
                return orderManager.possibleDrinks[i];
            }
        }

        return false;
    }


    public void Spill(float amount)
    {
        Debug.Log("Spilling amount: " + amount);
        currentAmount = Mathf.Max(currentAmount - amount, 0f);
        Debug.Log("Current Amount:" + currentAmount);
    }
}
