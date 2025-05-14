using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private MixingCup cup;
    private CustomerMovement customer;
    private OrderManager manager;

    public List<Recipes> order = new List<Recipes>();
    public float patiance;
    public int amountOfOrders;

    public Slider patienceSlider;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();

        for (int i = 0; i < amountOfOrders; i++)
        {
            manager.GeneratingOrder();
            order.Add(manager.orderGiven);
            manager.activeOrders.Add(this);
        }


        patienceSlider.maxValue = patiance;
        StartCoroutine(customer.LeaveAfterTime(patiance));
    }

    private void Update()
    {
        patiance -= Time.deltaTime;
        patienceSlider.value = patiance;
        if (patiance < 0)
        {
            patienceSlider.value = 0;
            patiance = 0;
        }
    }
    
    public void RemoveFromActiveOrderes()
    {
        NoMoreOrders();

        for (int i = 0; i < manager.activeOrders.Count; i++)
        {
            if (CompareOrder())
            {
                manager.CompleteOrder(manager.activeOrders[i], customer);
            }
        }
    }

    public void NoMoreOrders() 
    {
        if(order.Count <= 0)
        {
            Debug.Log("ima leave");
            customer.Leave();
        }
    }

    public bool CompareOrder()
    {
        cup.cupIngredientes.Sort();
        for (int i = 0; i < order.Count; i++)
        {
            order[i].requiredIngredientes.Sort();

            if (order[i].requiredIngredientes.SequenceEqual(cup.cupIngredientes))
            {
                Debug.Log(order[i].name);
                order.RemoveAt(i);
                return order[i];
            }
        }
        return false;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
            //CheckOrder();
            RemoveFromActiveOrderes();
        }
    }
}
