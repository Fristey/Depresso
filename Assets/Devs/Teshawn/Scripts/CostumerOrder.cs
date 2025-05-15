using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private MixingCup cup;
    private CustomerMovement customer;
    private OrderManager manager;

    public List<Recipes> order;
    public List<string> orderText;
    public float patiance;
    public int amountOfOrders;

    public Slider patienceSlider;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        order = new List<Recipes>();
        Debug.Log(manager);
        for (int i = 0; i < amountOfOrders; i++)
        {
            manager.GeneratingOrder();
            order.Add(manager.orderGiven);
            orderText.Add(manager.orderGiven.nameOfDrink);
        }
            manager.activeOrders.Add(this);


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
    
    /// <summary>
    /// this removes active orders out the list of the costumer
    /// uses the manager component and the active orders in the  OderManager  
    /// it also uses the CompleteOrder from the OrderManager
    /// </summary>
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
            manager.CompleteOrder(this,customer);
            customer.Leave();
        }
    }

    /// <summary>
    /// sorts the cup its list in Alphabetical order
    /// then its loops through the list of orders its count sorts every order its ingredients and compairs them if it maches it removes that one and returns the order
    /// </summary>
    /// <returns>the order that has been made or false</returns>
    public bool CompareOrder()
    {
        cup.cupIngredientes.Sort();
        for (int i = 0; i < order.Count; i++)
        {
            order[i].requiredIngredientes.Sort();

            if (order[i].requiredIngredientes.SequenceEqual(cup.cupIngredientes))
            {
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
            RemoveFromActiveOrderes();
        }
    }
}
