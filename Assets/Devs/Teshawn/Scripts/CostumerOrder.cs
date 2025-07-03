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
    private PointsManager pointsManager;

    public List<Recipes> costumerOrders;
    public List<string> orderText;
    public float patiance;
    public int amountOfOrders;
    public int currencyGiven;
    public int maxCurrencyGiven;
    private int randomSatisfactionMode = Enum.GetValues(typeof(SatisfactionType)).Length;

    public bool pointDecreaceStop;
    public bool isWaiting;

    public bool wilSpill = false;

    [SerializeField] private float extraCurrency;
    [SerializeField] private int maxExtraCurrency;
    public float extraPatience;
    [SerializeField] private float speedBonusTimer;

    public Slider patienceSlider;

    [SerializeField] private GameObject emptyCup;

    private GameManager gameManager;
    private int customerPoints;


    private void Awake()
    {
        manager = FindFirstObjectByType<OrderManager>();
        customer = GetComponent<CustomerMovement>();
        currencyManager = FindFirstObjectByType<CurrencyManager>();
        pointsManager = FindFirstObjectByType<PointsManager>();
        costumerOrders = new List<Recipes>();
    }
    private void Start()
    {
        int randomMode = UnityEngine.Random.Range(0, randomSatisfactionMode);
        type = (SatisfactionType)Enum.Parse(typeof(SatisfactionType), randomMode.ToString());
        gameManager = GameManager.Instance;


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
        //for testing
        amountOfOrders = gameManager.dayCycle.days[gameManager.dayCycle.currentDayIndex].maxOrdersPerCustomer;
        //for playing
        amountOfOrders = UnityEngine.Random.Range(1, gameManager.dayCycle.days[gameManager.dayCycle.currentDayIndex].maxOrdersPerCustomer + 1);
        for (int i = 0; i < amountOfOrders; i++)
        {
            manager.GeneratingOrder();
            costumerOrders.Add(manager.orderGiven);
            orderText.Add(manager.orderGiven.nameOfDrink);
            currencyGiven += costumerOrders[i].price;
        }
        manager.activeOrders.Add(this);

        patienceSlider.maxValue = patiance;

        isWaiting = true;

        customerPoints = 20 * amountOfOrders;
    }

    private void Update()
    {
        if (type == SatisfactionType.speed)
        {
            GenerateExtraSpeedPoints(extraCurrency);
        }

        if ((customer.currentState == CustomerMovement.CustomerState.Waiting || customer.currentState == CustomerMovement.CustomerState.Sitting) && gameManager.gameState == GameStates.playingDay)
        {
            patiance -= Time.deltaTime;

            if (patiance < 0)
            {
                patienceSlider.value = 0;
                patiance = 0;
                isWaiting = false;
            }
        }

        patienceSlider.value = patiance;
        FailedTime();
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
        cup.currentAmount = 0;
        pointsManager.AddPoints(customerPoints);
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
        cupFillCurrency = Mathf.FloorToInt(cup.currentAmount);
        for (int i = 0; i < cup.currentAmount; i++)
        {
            currencyGiven += cupFillCurrency + i;
            cup.currentAmount = 0;
        }
        currencyManager.AddCurrency(currencyGiven);
        currencyGiven = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MixingCup>() != null)
        {
            VisualSwapper swapper = collision.gameObject.GetComponent<VisualSwapper>();
            cup = collision.gameObject.GetComponent<MixingCup>();
            if (cup.drinkToserve != null)
            {
                for (int i = 0; i < costumerOrders.Count; i++)
                {
                    if (costumerOrders.Contains(cup.drinkToserve))
                    {
                        //Elger: switch the cup back to the blank cup
                        swapper.ResetVisual();

                        //before setting it to null (Emptying the cup)


                        collision.gameObject.GetComponent<MixingCup>().drinkToserve = null;
                        if (wilSpill)
                        {
                            manager.GeneratingOrder();
                            costumerOrders.Add(manager.orderGiven);
                            orderText.Add(manager.orderGiven.nameOfDrink);
                            patiance += 5f;
                            Debug.Log("spilled");
                        }

                        costumerOrders.Remove(costumerOrders[i]);
                        orderText.Remove(cup.drinkName);
                        collision.gameObject.GetComponent<MixingCup>().currentAmount = 0;
                        collision.gameObject.GetComponent<MixingCup>().cupIngredientes.Clear();
                        patiance += 5f;
                    }
                }
            }
            if (costumerOrders.Count < 1)
                NoMoreOrders();
        }
    }
}