using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private MixingCup cup;
    private CustomerMovement customer;
    private OrderManager manager;

    public Recipes order;
    public float patiance;

    public Slider patienceSlider;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        manager.GeneratingOrder();
        order = manager.orderGiven;
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
    public void CheckOrder()
    {
        for (int i = 0; i < manager.activeOrders.Count; i++)
        {
            if (CompareOrder())
            {
                manager.CompleteOrder(this,customer);
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cup"))
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
            CheckOrder();
        }
    }
}
