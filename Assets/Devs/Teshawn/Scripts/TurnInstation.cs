using System.Collections.Generic;
using UnityEngine;

public class TurnInstation : MonoBehaviour
{
    private OrderManager orderManager;
    [SerializeField] private List<Recipes> turnInRecipe;
    public MixingCup cups;

    void Start()
    {
        orderManager = FindAnyObjectByType<OrderManager>();
    }

    public void TurnInOrders()
    {
        for (int i = 0; i < orderManager.activeOrders.Count; i++)
        {
            CustomerOrder order = orderManager.activeOrders[i];

            for (int j = 0; j < order.costumerOrders.Count; j++)
            {
                if (turnInRecipe.Contains(order.costumerOrders[j]))
                {
                    turnInRecipe.Remove(order.costumerOrders[j]);
                    order.costumerOrders.RemoveAt(j);
                }
            }
            if (order.costumerOrders.Count <= 0)
            {
                orderManager.CompleteOrder(order);
                order.NoMoreOrders();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cups = collision.gameObject.GetComponent<MixingCup>();
            turnInRecipe.Add(cups.drinkToserve);
            TurnInOrders();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (cups != null) 
        {
            cups = null;
        }

    }
}
