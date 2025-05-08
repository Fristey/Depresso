using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{

    public Recipes order;

    public float patiance;

    private OrderManager manager;

    [SerializeField] private MixingCup cup;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        manager.GeneratingOrder();
        order = manager.orderGiven;
        manager.activeOrders.Add(order);
    }

    private void Update()
    {
        CheckOrder();
        Debug.Log(CompareOrder());
    }

    public void CheckOrder()
    {
        for (int i = 0; i < manager.activeOrders.Count; i++)
        {
            order = manager.activeOrders[i];
            if (CompareOrder())
            {
                manager.CompleteOrder();
            }
        }
    }

    public bool CompareOrder()
    {
        if (cup.cupIngredientes.Count != order.requiredIngredientes.Count)
        {
            return false;
        }

        for (int i = 0; order.requiredIngredientes.Count > 0; i++)
        {
            if (order.requiredIngredientes[i] != cup.cupIngredientes[i])
            {
                return false;
            }
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
        }
    }
}
