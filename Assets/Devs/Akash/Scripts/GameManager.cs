using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameStates
{ 
    tutorial,
    playingDay,
    inbetweenDays
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public Daycycle dayCycle;
    private float dayTimer = 0f;

    //public bool hasDayStarted = false;

    public GameStates gameState = GameStates.playingDay;
    private GameStates returnState = GameStates.playingDay;

    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Animator doorCamAnimator;

    private CamSwapManager camSwapManager;

    [SerializeField] private AudioSource normalMusic;
    [SerializeField] private AudioSource restMusic;

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

        camSwapManager = FindFirstObjectByType<CamSwapManager>();
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
    }

    private void StartDay(int dayIndex)
    {
        dayCycle.StartDay(dayIndex);
        dayTimer = 0f;

        gameState = GameStates.playingDay;

        AudioSwap(false);

        doorAnimator.SetBool("Open", false);
        camSwapManager.isLookingAtDoor = false;
        doorCamAnimator.SetBool("Zoom", false);
    }

    private void EndDay()
    {
        PointsManager points = GameObject.FindFirstObjectByType<PointsManager>();
        Debug.Log(points);

        if (points != null)
        {
            points.ShowPoints();
        }

        AudioSwap(true);

        gameState = GameStates.inbetweenDays;

        if (EventManager.instance != null)
        {
            EventManager.instance.ClearEvents();
        }

        RemoveAllCustomers();

        doorAnimator.SetBool("Open", true);
        Debug.Log("Day has ended");

        TutorialManager.instance.StartTutorial("EndDay");
    }

    public void ClickedDoor()
    {
        PointsManager points = GameObject.FindFirstObjectByType<PointsManager>();

        if (points != null)
        {
            points.StopShowing();
        }

        if (gameState == GameStates.playingDay)
        {
            return;
        } else if (gameState == GameStates.inbetweenDays)
        {
            camSwapManager.isLookingAtDoor = true;

            doorAnimator.SetTrigger("FullyOpen");
            doorCamAnimator.SetBool("Zoom",true);
        }

    }

    public void DoorAnimationFinished()
    {
        int nextDay = dayCycle.currentDayIndex + 1;

        if (nextDay < dayCycle.days.Count)
        {
            StartDay(nextDay);
        }
        else
        {
            SceneManager.LoadScene("EndScene");
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
        CustomerMovement.waitingCustomers.Clear();
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

    private void AudioSwap(bool restSoundActive)
    {
        if(restSoundActive)
        {
            restMusic.Play();
            normalMusic.Stop();
        } else
        {
            normalMusic.Play();
            restMusic.Stop();
        }
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
        [SerializeField] public int maxOrdersPerCustomer;
    }
    [SerializeField] public List<Day> days = new List<Day>();
    [SerializeField] public int currentDayIndex = 0;

    public void StartDay(int dayNumber)
    {

        currentDayIndex = dayNumber;

        Day currentDay = days[currentDayIndex];

        if (EventManager.instance != null)
        {
            EventManager.instance.SetDayEvents(days[dayNumber].temporaryEvents, days[dayNumber].permanentEvents, days[dayNumber].eventAmount, days[dayNumber].dayDuration);

            EventManager.instance.StartEvents();
        }

        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.SetSpawnSettings(currentDay.customerSpawnTimer, currentDay.maxCustomers);
        }
    }

    public Day GetCurrentDay()
    {
        return days[currentDayIndex];
    }
}