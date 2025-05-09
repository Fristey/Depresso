using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private MixingCup cup;

    private OrderManager manager;

    CustomerMovement movement;

    public Recipes order;
    public float patiance = 600f;

    private void Start()
    {
        movement = GetComponent<CustomerMovement>();
        manager = FindFirstObjectByType<OrderManager>();
        manager.GeneratingOrder();
        order = manager.orderGiven;
        manager.activeOrders.Add(this);
    }

    private void Update()
    {
        Patience();
    }

    public void CheckOrder()
    {
        for (int i = 0; i < manager.activeOrders.Count; i++)
        {
            if (CompareOrder())
            {
                manager.CompleteOrder(this);
            }
        }
    }

    public bool CompareOrder()
    {
        cup.cupIngredientes.Sort();
        order.requiredIngredientes.Sort();

        if (cup.cupIngredientes.Count != order.requiredIngredientes.Count)
        {
            return false;
        }

        for (int i = 0; order.requiredIngredientes.Count > 0; i++)
        {
            if (order.requiredIngredientes[i] == cup.cupIngredientes[i])
            {
                return true;
            }
        }
       return false;
    }

    private void Patience()
    {
        if (order != null)
        {
            patiance -= Time.deltaTime;
        }

        if(patiance <= 0)
        {
            manager.FailOrder(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
            CheckOrder();
        }
    }
}
