using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Daycycle dayCycle;
    private float dayTimer = 0f;

    private bool hasDayStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        dayTimer += Time.deltaTime;
        float currentDayDuration = dayCycle.GetCurrentDay().dayDuration;

        if(dayTimer >= currentDayDuration)
        {
            hasDayStarted = false;
            dayTimer = 0f;
            StartNextDay();
        }

        Debug.Log(currentDayDuration);
    }

    private void Start()
    {
        dayCycle.StartDay(0);
        Debug.Log("Starting day: " + dayCycle.currentDayIndex);
    }

    public void StartNextDay()
    { 
        int nextDay = dayCycle.currentDayIndex+1;
        Debug.Log("Next day: " + nextDay);
        if (nextDay < dayCycle.days.Count)
        {
            dayCycle.StartDay(nextDay);
            RemoveAllCustomers();
        }
    }

    public void StartNewDay(int dayNumber)
    {
        dayCycle.StartDay(dayNumber);
        dayTimer = 0f;
        hasDayStarted = true;
    }


    private void RemoveAllCustomers()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
        foreach (GameObject customer in customers)
        {
            Destroy(customer);
        }

        CustomerSpawner.Instance.currentCustomerCount = 0;

        CustomerMovement.usedStools.Clear();
        CustomerMovement.usedWaitSpots.Clear();
    }

}


[System.Serializable]
public class Daycycle
{
    [System.Serializable]
    public class Day
    {
        public float customerSpawnTimer = 3f;
        public int maxCustomers = 5;
        public float dayDuration = 60f;

        [SerializeField] private List<GameObject> temporaryEvents = new List<GameObject>();
        [SerializeField] private List<GameObject> permanentEvents = new List<GameObject>();
    }
    [SerializeField] public List<Day> days = new List<Day>();
    [SerializeField] public int currentDayIndex = 0;

    public void StartDay(int dayNumber)
    {
        currentDayIndex = dayNumber; 

        Day currentDay = days[currentDayIndex];

        if (CustomerSpawner.Instance != null)
        {
            Debug.Log(currentDay.customerSpawnTimer);
            CustomerSpawner.Instance.SetSpawnSettings(currentDay.customerSpawnTimer, currentDay.maxCustomers);
        }

        //If statement voor de eventmanager (?)


    }

    public Day GetCurrentDay()
    {
        return days[currentDayIndex];
    }




}