using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SatisfactionType { scene, speed, none }

public class CustomerOrder : MonoBehaviour
{
    public SatisfactionType type;


    [SerializeField] private TurnInstation turnInStaton;
    private CustomerMovement customer;
    private OrderManager manager;
    private CurrencyManager currencyManager;

    public List<Recipes> costumerOrders;
    public List<string> orderText;
    public float patiance;
    public int amountOfOrders;
    public int currencyGiven = 20;
    public int maxCurrencyGiven;
    private int randomSatisfactionMode = Enum.GetValues(typeof(SatisfactionType)).Length;
    private int enumSize = Enum.GetValues(typeof(Size)).Length;

    public bool pointDecreaceStop;
    public bool isWaiting;

    [SerializeField] private float extraCurrency;
    [SerializeField] private int maxExtraCurrency;
    public float extraPatience = 5;
    [SerializeField] private float speedBonusTimer;

    public Slider patienceSlider;

    private void Start()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        turnInStaton = FindFirstObjectByType<TurnInstation>();
        costumerOrders = new List<Recipes>();
        int randomMode = UnityEngine.Random.Range(0, randomSatisfactionMode);
        type = (SatisfactionType)Enum.Parse(typeof(SatisfactionType), randomMode.ToString());


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
            costumerOrders.Add(manager.orderGiven);
            orderText.Add(manager.orderGiven.nameOfDrink);
        }
        manager.activeOrders.Add(this);
        for (int i = 0; i < costumerOrders.Count; i++)
        {
            int randomSize = UnityEngine.Random.Range(0, enumSize);
            costumerOrders[i].drinkSize = (Size)Enum.Parse(typeof(Size), randomSize.ToString());
        }


        StartCoroutine(customer.LeaveAfterTime(patiance));
        patienceSlider.maxValue = patiance;
    }

    private void Update()
    {
        if (type == SatisfactionType.speed)
        {
            GenerateExtraSpeedPoints(extraCurrency);
        }

        if (isWaiting)
        {
            patiance -= Time.deltaTime;
            patienceSlider.value = patiance;
            if (patiance < 0)
            {
                patienceSlider.value = 0;
                patiance = 0;
            }
        }

        FailedTime();
    }

    public void NoMoreOrders()
    {
        Debug.Log("leaving");
        isWaiting = false;
        if (type == SatisfactionType.speed)
        {
            Debug.Log("speed");
            currencyManager.AddCurrency(maxCurrencyGiven);
        }
        else
        {
            Debug.Log("no speed");
            GenerateExtraCupFillCurrency(currencyGiven);
        }
        customer.Leave();
    }

    public void FailedTime()
    {
        if (patiance <= 0)
        {
            manager.FailOrder(this, customer);
        }
    }

    private void GenerateExtraSpeedPoints(float extraCurrency)
    {

        if (!pointDecreaceStop)
        {
            speedBonusTimer += Time.deltaTime / (patiance * 0.5f);
            extraCurrency = Mathf.Lerp(maxExtraCurrency, 0, speedBonusTimer);
            extraCurrency = Mathf.FloorToInt(extraCurrency);
            maxCurrencyGiven = (int)extraCurrency + currencyGiven;
        }

        if (speedBonusTimer > 1)
        {
            pointDecreaceStop = true;
        }
    }

    private void GenerateExtraCupFillCurrency(int cupFillCurrency)
    {
        cupFillCurrency = Mathf.FloorToInt(turnInStaton.cups.currentAmount * 2);
        for (int i = 0; i < turnInStaton.cups.currentAmount; i++)
        {
            currencyGiven += cupFillCurrency + i;
            turnInStaton.cups.currentAmount = 0;
        }
        currencyManager.AddCurrency(currencyGiven);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Cup"))
    //    {
    //        turnInStaton = collision.gameObject.GetComponent<MixingCup>();
    //        NoMoreOrders();
    //    }
    //}
}