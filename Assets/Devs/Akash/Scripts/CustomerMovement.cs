using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public enum CustomerState { Waiting, Walking, Sitting, Leaving }

    public CustomerState currentState = CustomerState.Walking;

    public NavMeshAgent navMeshAgent;
    public OrderManager orderManager;
    public CustomerOrder order;
    public CapsuleCollider sitCollider, standingCollider;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private GameObject Counter;
    [SerializeField] Animator animator;
    private List<GameObject> waitPoints;
    private List<GameObject> counterStools;
    private GameObject exitPoint;
    private GameObject spawnPoint;
    private GameObject currentSpot;
    private float elapsedTime;

    public static List<GameObject> usedStools = new List<GameObject>();
    public static List<GameObject> usedWaitSpots = new List<GameObject>();
    public static List<CustomerMovement> waitingCustomers = new List<CustomerMovement>();

    private bool startLeaving;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        orderManager = FindAnyObjectByType<OrderManager>();
        order = GetComponent<CustomerOrder>();
        
    }

    private void Start()
    {
        elapsedTime = order.patiance;
        counterStools = CustomerManager.Instance.counterStools;
        waitPoints = CustomerManager.Instance.waitPoints;
        exitPoint = CustomerManager.Instance.exitPoint;
        spawnPoint = CustomerManager.Instance.spawnPoint;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        navMeshAgent.speed = walkSpeed;
        currentState = CustomerState.Walking;
        TryFindingFreeSpot();



    }

    private void Update()
    {
        if (currentSpot != null && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && currentState != CustomerState.Sitting && currentState != CustomerState.Leaving)
        {
            if (counterStools.Contains(currentSpot))
            {
                navMeshAgent.isStopped = true;

                if (!startLeaving)
                {
                    startLeaving = true;
                    currentState = CustomerState.Sitting;
                    /*                    animator.SetTrigger("SittingDown");*/
                    //StartCoroutine(LeaveAfterTime(Random.Range(5f, 10f)));
                }
            }
        }

        if (currentState == CustomerState.Sitting)
        {
            animator.SetTrigger("sitDown");
            animator.SetBool("Sitting", true);

        }

        if (currentState == CustomerState.Waiting && navMeshAgent.velocity.magnitude == 0)
        {
            animator.SetBool("Waiting", true);
        }
        else
        {
            animator.SetBool("Waiting", false);
        }


        elapsedTime -= Time.deltaTime;


        if (elapsedTime < 5)
        {
            animator.SetBool("Sitting", false);
            animator.SetTrigger("standUp");
        }

        float animSpeed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("walkSpeed", animSpeed);

        if (animator != null)
            ColliderSwap();
    }

    private void TryFindingFreeSpot()
    {
        foreach (var stool in counterStools)
        {
            if (!usedStools.Contains(stool))
            {
                currentSpot = stool;
                usedStools.Add(stool);
                navMeshAgent.SetDestination(currentSpot.transform.position);
                animator.SetBool("isWalking", true);
                currentState = CustomerState.Walking;
                return;
            }
        }

        foreach (var waitSpot in waitPoints)
        {
            if (!usedWaitSpots.Contains(waitSpot))
            {
                currentSpot = waitSpot;
                usedWaitSpots.Add(waitSpot);
                navMeshAgent.SetDestination(currentSpot.transform.position);
                currentState = CustomerState.Waiting;
                waitingCustomers.Add(this);
                return;
            }
        }
        navMeshAgent.isStopped = true;
    }

    public IEnumerator LeaveAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (currentSpot != null)
        {
            if (counterStools.Contains(currentSpot))
            {
                usedStools.Remove(currentSpot);
                FreeStoolCheck();
            }

            if (waitPoints.Contains(currentSpot))
            {
                usedWaitSpots.Remove(currentSpot);
            }
        }
        waitingCustomers.Remove(this);
        ReOrderQueue();
        orderManager.FailOrder(order, this);
        //Leave();
    }

    private void FreeStoolCheck()
    {
        if (waitingCustomers.Count == 0)
        {
            return;
        }

        foreach (var stool in counterStools)
        {
            if (!usedStools.Contains(stool))
            {
                CustomerMovement nextCustomer = waitingCustomers[0];
                waitingCustomers.RemoveAt(0);

                if (nextCustomer.currentSpot != null && waitPoints.Contains(nextCustomer.currentSpot))
                {
                    usedWaitSpots.Remove(nextCustomer.currentSpot);
                }

                nextCustomer.currentSpot = stool;
                usedStools.Add(stool);
                nextCustomer.navMeshAgent.isStopped = false;
                animator.SetBool("Waiting", false);
                nextCustomer.navMeshAgent.SetDestination(stool.transform.position);
                animator.SetBool("isWalking", true);
                nextCustomer.currentState = CustomerState.Walking;

                break;
            }
        }
    }

    public void Leave()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("Leaving", true);
        currentState = CustomerState.Leaving;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(exitPoint.transform.position);
        Destroy(gameObject, 5f);
        if (currentSpot != null && counterStools.Contains(currentSpot))
        {
            usedStools.Remove(currentSpot);
            FreeStoolCheck();
        }

        CustomerSpawner.Instance.currentCustomerCount -= 1;
    }

    private void ReOrderQueue()
    {
        for (int i = 0; i < waitingCustomers.Count; i++)
        {
            CustomerMovement customer = waitingCustomers[i];
            GameObject targetWaitSpot = CustomerManager.Instance.waitPoints[i];

            if (customer.currentSpot != targetWaitSpot)
            {
                if (customer.currentSpot != null)
                {
                    usedWaitSpots.Remove(customer.currentSpot);
                }
                customer.currentSpot = targetWaitSpot;
                usedWaitSpots.Add(targetWaitSpot);
                customer.navMeshAgent.isStopped = false;
                customer.navMeshAgent.SetDestination(targetWaitSpot.transform.position);


            }
        }
    }

    private void ColliderSwap()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle Astronaut"))
        {
            sitCollider.enabled = true;
            standingCollider.enabled = false;
        }
        else
        {
            sitCollider.enabled = false;
            standingCollider.enabled = true;
        }
    }
}