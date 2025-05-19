using System.Linq;
using UnityEngine;

// press interact to mix drink or something discuss with team
public class MixingStation : MonoBehaviour
{
    private MixingCup cup;
    private OrderManager orderManager;

    private void Start()
    {
        orderManager = FindFirstObjectByType<OrderManager>();
    }

    private void Update()
    {
        CreateDrink();
    }

    public bool CreateDrink()
    {
        if (cup != null)
        {
            cup.ingredientesNames.Sort();
            for (int i = 0; i < orderManager.possibleDrinks.Count; i++)
            {
                orderManager.possibleDrinks[i].requiredIngredientes.Sort();

                if (cup.cupIngredientes.SequenceEqual(orderManager.possibleDrinks[i].requiredIngredientes))
                {
                    cup.drinkToserve = orderManager.possibleDrinks[i];
                    cup.ingredientesNames.Clear();
                    cup.cupIngredientes.Clear();
                    return orderManager.possibleDrinks[i];
                }
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (cup == null)
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
        }
    }
}
