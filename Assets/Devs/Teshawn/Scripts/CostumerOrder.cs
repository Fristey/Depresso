using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SatisfactionType { scene, speed, none }

public class CustomerOrder : MonoBehaviour
{
    public SatisfactionType type;

    [SerializeField] private MixingCup cup;
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

    public bool pointDecreaceStop;
    public bool isWaiting;

    [SerializeField] private float extraCurrency;
    [SerializeField] private int maxExtraCurrency;
    public float extraPatience;
    [SerializeField] private float speedBonusTimer;

    public Slider patienceSlider;

    private void Awake()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        costumerOrders = new List<Recipes>();
    }
    private void Start()
    {
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

        amountOfOrders = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < amountOfOrders; i++)
        {
            manager.GeneratingOrder();
            costumerOrders.Add(manager.orderGiven);
            orderText.Add(manager.orderGiven.nameOfDrink);
        }
        manager.activeOrders.Add(this);
        
        patienceSlider.maxValue = patiance;

        isWaiting = true;
    }

    private void Update()
    {
        if (type == SatisfactionType.speed)
        {
            GenerateExtraSpeedPoints(extraCurrency);
        }

        if (customer.currentState == CustomerMovement.CustomerState.Waiting || customer.currentState == CustomerMovement.CustomerState.Sitting)
        {
            patiance -= Time.deltaTime;
            patienceSlider.value = patiance;
            if (patiance < 0)
            {
                patienceSlider.value = 0;
                patiance = 0;
                isWaiting = false;
            }
        }

        FailedTime();
    }

    public void CompareOrder()
    {
        if (costumerOrders.Contains(cup.drinkToserve))  
        {
            costumerOrders.Remove(cup.drinkToserve);
            orderText.Remove(cup.drinkToserve.nameOfDrink);
        }

        if(costumerOrders.Count <= 0)
        {
            NoMoreOrders();
        }
    }

    public void NoMoreOrders()
    {

        if (type == SatisfactionType.speed)
        {
            currencyManager.AddCurrency(maxCurrencyGiven);
        }
        else
        {
            GenerateExtraCupFillCurrency(currencyGiven);
        }
        customer.Leave();
    }

    public void FailedTime()
    {
        if (!isWaiting)
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
        cupFillCurrency = Mathf.FloorToInt(cup.currentAmount * 2);
        for (int i = 0; i < cup.currentAmount; i++)
        {
            currencyGiven += cupFillCurrency + i;
            cup.currentAmount = 0;
        }
        currencyManager.AddCurrency(currencyGiven);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            cup = collision.gameObject.GetComponent<MixingCup>();
            CompareOrder();
        }
    }
}