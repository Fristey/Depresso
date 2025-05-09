using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<Recipes> possibleDrinks = new List<Recipes>();

    public Recipes orderGiven;

    [SerializeField] private float customerCooldown;

    public List<CustomerOrder> activeOrders = new List<CustomerOrder>();


    public void GeneratingOrder()
    {
        int givenOrder = Random.Range(0, possibleDrinks.Count);

        orderGiven = possibleDrinks[givenOrder];
    }

    public void CompleteOrder(CustomerOrder order)
    {
        int customerOrderToRemove = activeOrders.IndexOf(order);
        activeOrders.RemoveAt(customerOrderToRemove);
    }

    public void FailOrder(CustomerOrder order)
    {
        int customerOrderToRemove = activeOrders.IndexOf(order);
        activeOrders.RemoveAt(customerOrderToRemove);
    }
}
