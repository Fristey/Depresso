using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum GameStates
{ 
    tutorial,
    playingDay,
    inbetweenDays
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Daycycle dayCycle;
    private float dayTimer = 0f;

    //public bool hasDayStarted = false;

    public GameStates gameState = GameStates.playingDay;
    private GameStates returnState = GameStates.playingDay;

    //Testing
    public bool trigger = false;

    private TutorialManager tutorialManager;

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
        if (gameState == GameStates.playingDay)
        {
            dayTimer += Time.deltaTime;
            float currentDayDuration = dayCycle.GetCurrentDay().dayDuration;

            if (dayTimer >= currentDayDuration)
            {
                /*            hasDayStarted = false;
                            dayTimer = 0f;
                            StartNextDay();*/
                EndDay();
            }
        }
    }

    private void Start()
    {
        StartDay(0);

        tutorialManager = TutorialManager.instance;
    }

/*    public void StartNextDay()
    {
        if (EventManager.instance != null)
        {
            EventManager.instance.ClearEvents();
        }

        int nextDay = dayCycle.currentDayIndex + 1;
        if (nextDay < dayCycle.days.Count)
        {
            dayCycle.StartDay(nextDay);
            RemoveAllCustomers();
        }
    }*/

    private void StartDay(int dayIndex)
    {

        dayCycle.StartDay(dayIndex);
        dayTimer = 0f;
        //hasDayStarted = true;
        gameState = GameStates.playingDay;
    }

    private void EndDay()
    {
        //hasDayStarted = false;
        gameState = GameStates.inbetweenDays;

        if (EventManager.instance != null)
        {
            EventManager.instance.ClearEvents();
        }

        RemoveAllCustomers();
    }

    public void ClickedDoor()
    {
        if (gameState == GameStates.playingDay)
        {
            return;
        }
        int nextDay = dayCycle.currentDayIndex + 1;

        if (nextDay < dayCycle.days.Count)
        {
            StartDay(nextDay);
        }
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

    //- Tutorial code -//

    public void SetGameState(GameStates newState)
    {
        returnState = gameState;
        gameState = newState;
    }

    public void ReturnGameState()
    {
        gameState = returnState;
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

        [SerializeField] public List<GameObject> temporaryEvents = new List<GameObject>();
        [SerializeField] public List<GameObject> permanentEvents = new List<GameObject>();
        public int eventAmount;
    }
    [SerializeField] public List<Day> days = new List<Day>();
    [SerializeField] public int currentDayIndex = 0;

    public void StartDay(int dayNumber)
    {
        Debug.Log("Start the day");

        currentDayIndex = dayNumber;

        Day currentDay = days[currentDayIndex];

        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.SetSpawnSettings(currentDay.customerSpawnTimer, currentDay.maxCustomers);
        }

        if (EventManager.instance != null)
        {
            EventManager.instance.SetDayEvents(days[dayNumber].temporaryEvents, days[dayNumber].permanentEvents, days[dayNumber].eventAmount, days[dayNumber].dayDuration);

            EventManager.instance.StartEvents();
        }


    }

    public Day GetCurrentDay()
    {
        return days[currentDayIndex];
    }
}