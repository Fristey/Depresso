using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CustomerMovement : MonoBehaviour
{
    public enum CustomerState { Waiting, Walking, Interacting }

    public CustomerState currentState = CustomerState.Walking;

    public NavMeshAgent navMeshAgent;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private GameObject Counter;
    [SerializeField] private List<GameObject> waitPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> counterStools = new List<GameObject>();

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        navMeshAgent.speed = walkSpeed;
        currentState = CustomerState.Walking;
    }

    private void Update()
    {
        switch (currentState)
        {
            case CustomerState.Walking:
                WalkToCounter();
                break;
            case CustomerState.Interacting:
                // Handle interaction logic here
                break;
            case CustomerState.Waiting:
                WaitInLine();
                break;
        }
    }

    private void WalkToCounter()
    {
        if (Counter != null)
        {
            navMeshAgent.SetDestination(Counter.transform.position);
            if (Vector3.Distance(transform.position, Counter.transform.position) < 1f)
            {
                currentState = CustomerState.Interacting;
                GetAvailableStoolIndex();
                // Optionally, you can stop the agent when it reaches the destination
                navMeshAgent.isStopped = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Counter"))
        {
            currentState = CustomerState.Interacting;
            // Optionally, you can stop the agent when it reaches the destination
            navMeshAgent.isStopped = true;
        }
    }

    private void WaitInLine()
    {
        if (waitPoints.Count > 0)
        {
            foreach (GameObject waitPoint in waitPoints)
            {
                navMeshAgent.SetDestination(waitPoint.transform.position);
                if (Vector3.Distance(transform.position, waitPoint.transform.position) < 1f)
                {
                    currentState = CustomerState.Waiting;
                    // Optionally, you can stop the agent when it reaches the destination
                    navMeshAgent.isStopped = true;
                }
            }
        }
    }

    private int GetAvailableStoolIndex()
    {
        for (int i = 0; i < counterStools.Count; i++)
        {
            if (!isStoolUnavailable(i))
            {
                return i;
            }
        }
        currentState = CustomerState.Waiting;
        return -1; // No available stool found
        
    }


    private bool isStoolUnavailable(int index)
    {

        if(index >= counterStools.Count)
        {
            return true;
        }

        foreach (GameObject customer in counterStools)
        {
            if(customer.transform.position == counterStools[index].transform.position)
            {
                return true;
            }
        }
        return false;
    }
}


