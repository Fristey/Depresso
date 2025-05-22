using UnityEngine;

public class UnlockRecipe : MonoBehaviour
{

    private OrderManager orderManager;
    public Recipes recipe;


    void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
    }
   

    public void UnlockMoreRecipes()
    {
        if (!orderManager.possibleDrinks.Contains(recipe))
        {
            orderManager.possibleDrinks.Add(recipe);
        }
        else
        {
            Debug.Log("you have this drink");
        }
    }
}
