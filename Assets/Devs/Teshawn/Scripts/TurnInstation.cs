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

    public void CheckingOrder()
    {
        for (int i = 0; i < turnInRecipe.Count; i++)
        {
            orderManager.activeOrders[i].NoMoreOrders();
            orderManager.activeOrders[i].costumerOrders.Remove(turnInRecipe[i]);
            turnInRecipe.RemoveAt(i);
            Debug.Log(i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cups = collision.gameObject.GetComponent<MixingCup>();
            turnInRecipe.Add(cups.drinkToserve);

            CheckingOrder();
        }
    }
}
