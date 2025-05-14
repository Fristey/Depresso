using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

enum EventManagerStates
{
    Inactive,
    Waiting,
    Playing
}
public class EventManager : MonoBehaviour
{
    //(Explenation Concept) The gameManager must set the events for the current day each day these will then be saved here and executed by this script

    //Saving lists
    [SerializeField] private List<GameObject> tempEvents = new List<GameObject>();
    [SerializeField] private List<GameObject> playableTempEvents = new List<GameObject>();

    [SerializeField] private List<GameObject> permEvents = new List<GameObject>();

    [SerializeField] private GameObject curEvent;

    [Header("Stats")]
    [SerializeField] private EventManagerStates state;


    [SerializeField] private int eventAmount;
    [SerializeField] private float maxWait;
    [SerializeField] private float minWait;
    [SerializeField] private float totalTempEventDur;
    [SerializeField] private float playedTime;

    static EventManager instance;

    //Temp variables
    [Header("Temporary variables")]
    [SerializeField] private float dayDur;
    [SerializeField] private float eventDur;

    [SerializeField] private List<GameObject> PHtempEvents = new List<GameObject>();
    [SerializeField] private List<GameObject> PHpermEvents = new List<GameObject>();




    //Makking sure this is the only EventManager active
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetDayEvents(PHtempEvents, PHpermEvents, 3);

        state = EventManagerStates.Inactive;
    }
    private void Start()
    {
        StartEvents();
    }

    //Allowing the gameManager to set the days events and preparing these for the day (A kind of awake function)
    public void SetDayEvents(List<GameObject> curDayTempEvents, List<GameObject> curDayPermEvents, int curEventAmount)
    {
        tempEvents = new List<GameObject>(curDayTempEvents);
        playableTempEvents = new List<GameObject>(curDayTempEvents);

        permEvents = curDayPermEvents;

        eventAmount = curEventAmount;

        playedTime = 0;

        CalculateMaxWait();
    }

    private void CalculateMaxWait()
    {
        totalTempEventDur = eventDur * eventAmount;
        maxWait = (dayDur - totalTempEventDur - playedTime) / eventAmount;
    }

    //Allowing the gameManager to start the days events (A kind of start function)
    public void StartEvents()
    {
        for (int i = 0; i < permEvents.Count; i++)
        {
            Instantiate(permEvents[i], transform.position, Quaternion.identity);
        }

        StartCoroutine(EventCD());
    }

    private void Update()
    {
        switch (state)
        {
            case EventManagerStates.Waiting:
                if(eventAmount > 0)
                {
                    Debug.Log("StartEvent");

                    curEvent = Instantiate(SelectEvent(), transform.position, Quaternion.identity);
                    StartCoroutine(EventDur());
                }
                break;
            case EventManagerStates.Playing:
                break;
        }
    }

    //Allowing the gameManager to end current events (A kind of end function)
    public void ClearEvents()
    {
        state = EventManagerStates.Inactive;

        tempEvents.Clear();
        playableTempEvents.Clear();

        permEvents.Clear();
        curEvent = null;
    }
    
    private IEnumerator EventDur()
    {
        state = EventManagerStates.Playing;

        playedTime += eventDur;

        yield return new WaitForSeconds(eventDur);
        Debug.Log("Event ended");


        Destroy(curEvent);
        StartCoroutine (EventCD());
    }
    private IEnumerator EventCD()
    {
        state = EventManagerStates.Playing;

        Debug.Log("Timer started");

        float cd = Random.Range(minWait, maxWait);

        yield return new WaitForSeconds(cd);

        playedTime += cd;

        Debug.Log("TimerFinished");

        CalculateMaxWait();

        state = EventManagerStates.Waiting;
    }

    private GameObject SelectEvent()
    {
        int index = Random.Range(0, playableTempEvents.Count - 1);
        GameObject chosenEvent = playableTempEvents[index];

        playableTempEvents.RemoveAt(index);
        eventAmount--;

        if (playableTempEvents.Count == 0)
        {
            playableTempEvents = new List<GameObject>(tempEvents);
        }

        return chosenEvent;
    }
}
