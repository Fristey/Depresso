using System.Collections.Generic;
using UnityEngine;

public class TurnInstation : MonoBehaviour
{
    private OrderManager orderManager;
    [SerializeField] private List<Recipes> turnInRecipe;

    void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
    }

    public void CheckingOrder()
    {
        for (int i = 0; i < turnInRecipe.Count; i++)
        {
            if (orderManager.activeOrders[i].costumerOrders.Contains(turnInRecipe[i]))
            {
                orderManager.activeOrders[i].costumerOrders.Remove(turnInRecipe[i]);
                turnInRecipe.RemoveAt(i);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            turnInRecipe.Add(collision.gameObject.GetComponent<MixingCup>().drinkToserve);
            CheckingOrder();
        }
    }
}
