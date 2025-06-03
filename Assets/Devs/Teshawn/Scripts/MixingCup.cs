using Copying;
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

        if (drinkToserve != null)
        {
            if (Input.GetKeyUp(KeyCode.Q))
                Copy.CopyingComponents(drinkToserve.drink, this.gameObject);
        }
    }
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
