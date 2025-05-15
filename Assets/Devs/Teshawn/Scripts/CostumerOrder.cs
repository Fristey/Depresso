using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public enum SatisfactionType { scene, speed, none }

public class CustomerOrder : MonoBehaviour
{
    public SatisfactionType type;


    [SerializeField] private MixingCup cup;
    private CustomerMovement customer;
    private OrderManager manager;

    public List<Recipes> order;
    public List<string> orderText;
    public float patiance;
    public int amountOfOrders;
    public int currencyGiven = 20;
    public int maxCurrencyGiven;

    public bool pointDecreaceStop;

    [SerializeField] private float extraCurrency;
    [SerializeField] private int maxExtraCurrency;
    public float extraPatience = 5;
    [SerializeField] private float speedBonusTimer;

    public Slider patienceSlider;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        order = new List<Recipes>();
        speedBonusTimer = 0f;
        if (type == SatisfactionType.speed)
        {
            extraCurrency = maxExtraCurrency;
            maxCurrencyGiven = currencyGiven + (int)extraCurrency;

        }
        if (type == SatisfactionType.scene)
        {
            patiance += extraPatience;
        }

        //set random range later
        for (int i = 0; i < amountOfOrders; i++)
        {
            manager.GeneratingOrder();
            order.Add(manager.orderGiven);
            orderText.Add(manager.orderGiven.nameOfDrink);
        }
        manager.activeOrders.Add(this);


        StartCoroutine(customer.LeaveAfterTime(patiance));
        patienceSlider.maxValue = patiance;
    }

    private void Update()
    {
        if (type == SatisfactionType.speed)
        {
            GenerateExtraPoints(extraCurrency);
        }

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
        if (order.Count <= 0)
        {
            manager.CompleteOrder(this, customer);
            customer.Leave();
        }
    }

    public void FailedTime()
    {
        if (patiance <= 0)
        {
            Debug.Log("bu bye");
            manager.FailOrder(this, customer);
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

    private void GenerateExtraPoints(float extraCurrency)
    {

        if (!pointDecreaceStop)
        {
            speedBonusTimer += Time.deltaTime / (patiance * 0.5f);
            extraCurrency = Mathf.Lerp(maxExtraCurrency, 0, speedBonusTimer);
            extraCurrency = Mathf.FloorToInt(extraCurrency);
            maxCurrencyGiven = (int)extraCurrency + currencyGiven;
        }

        if(speedBonusTimer > 1)
        {
            pointDecreaceStop = true;
        }
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