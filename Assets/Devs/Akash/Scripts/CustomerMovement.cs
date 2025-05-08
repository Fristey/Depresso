using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public enum CustomerState { Waiting, Walking, Interacting }

    public CustomerState currentState = CustomerState.Walking;

    public NavMeshAgent navMeshAgent;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private GameObject Counter;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        navMeshAgent.speed = walkSpeed;
        currentState = CustomerState.Walking;
    }
}

