using System.Collections.Generic;
using UnityEngine;
public class OrderManager : MonoBehaviour
{
    public List<Recipes> possibleDrinks = new List<Recipes>();
    public List<CustomerOrder> activeOrders = new List<CustomerOrder>();
    public Recipes orderGiven;

    public float currencyFromCostumer;
    public void GeneratingOrder()
    {
        int givenOrder = Random.Range(0, possibleDrinks.Count);
        orderGiven = possibleDrinks[givenOrder];
    }


    public void CompleteOrder(CustomerOrder order)
    {
        int customerOrderToRemove = activeOrders.IndexOf(order);
        activeOrders.RemoveAt(customerOrderToRemove);

        if (order.type == SatisfactionType.speed)
        {
            order.pointDecreaceStop = true;
        }
    }

    public void FailOrder(CustomerOrder order, CustomerMovement customerMovement)
    {
        int customerOrderToRemove = activeOrders.IndexOf(order);
        activeOrders.RemoveAt(customerOrderToRemove);
        customerMovement.Leave();
    }
}
