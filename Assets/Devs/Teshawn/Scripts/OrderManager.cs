using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private List<Recipes> possibleDrinks = new List<Recipes>();

    public Recipes orderGiven;

    [SerializeField] private float customerCooldown;

    public List<Recipes> activeOrders = new List<Recipes>();


    public void GeneratingOrder()
    {
        int givenOrder = Random.Range(0, possibleDrinks.Count);

        orderGiven = possibleDrinks[givenOrder];
    }

    public void CompleteOrder()
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            activeOrders.Remove(activeOrders[i]);
        }
    }

    public void FailOrder()
    {
        //if time is up failed the order
    }
}
